using SocialMediaApp.BusinessLogic.Mapping;
using SocialMediaApp.DataAccess.Dtos.PostDto;
using SocialMediaApp.DataAccess.Entity;
using SocialMediaApp.DataAccess.Repositories.CategoryRepository;
using SocialMediaApp.DataAccess.Repositories.PostRepository;
using SocialMediaApp.DataAccess.Repositories.UserCategoryRepository;
using SocialMediaApp.DataAccess.Repositories.UserInteractionRepository;
using SocialMediaApp.DataAccess.Repositories.UserSeenPostRepository;

namespace SocialMediaApp.BusinessLogic.Services.FeedService
{
	public class FeedService : IFeedService
	{
		private readonly IPostRepository _postRepository;
		private readonly IUserCategoryRepository _userCategoryRepository;
		private readonly IUserInteractionRepository _userInteractionRepository;
		private readonly ICategoryRepository _categoryRepository;
		private readonly IUserSeenPostRepository _userSeenPostRepository;

		public FeedService(IPostRepository postRepository,
			IUserCategoryRepository userCategoryRepository,
			IUserInteractionRepository userInteractionRepository,
			ICategoryRepository categoryRepository,
			IUserSeenPostRepository userSeenPostRepository)
		{
			_postRepository = postRepository;
			_userCategoryRepository = userCategoryRepository;
			_userInteractionRepository = userInteractionRepository;
			_categoryRepository = categoryRepository;
			_userSeenPostRepository = userSeenPostRepository;
		}

		public List<PostResponseDto> GetPersonalizedFeed(int userId, int totalPosts = 20)
		{
			// 1) Preluăm preferințele userului
			var userCategoryPreferences = _userCategoryRepository.GetPreferencesByUser(userId);

			// 2) Filtrăm categoriile cu scor > 0 (pozitive)
			var positivePrefs = userCategoryPreferences
				.Where(ucp => ucp.Score > 0)
				.ToList();

			// 3) Calculăm câte postări rezervăm pentru diversitate (10% din total, cel puțin 1)
			int diversityCount = Math.Max(1, (int)Math.Ceiling(totalPosts * 0.1));
			int weightedPostCount = totalPosts - diversityCount;

			// 4) Preluăm lista de postări deja văzute (ID-urile lor)
			var seenPostIds = _userSeenPostRepository.GetSeenPostIds(userId);

			// 5) Dacă nu există preferințe pozitive, folosim fallback:
			//    (a) primele weightedPostCount postări din toate categoriile, nefiltrate de scor
			//    (b) diversityCount postări din postările rămase (non-preferate)
			if (!positivePrefs.Any())
			{
				// --- (a) primele weightedPostCount postări din toate categoriile (ne-văzute) ---
				var fallbackWeighted = _postRepository
					.GetAll()
					.Where(p => !seenPostIds.Contains(p.PostId))
					.OrderByDescending(p => p.CreatedAt)
					.Take(weightedPostCount)
					.ToList();

				// --- (b) diversity: postări din categoriile non-preferate ---
				//    deoarece nu avem preferințe pozitive, le luăm din toate categoriile (ne-filtrate)
				var fallbackDiversity = _postRepository
					.GetAll()
					.Where(p => !seenPostIds.Contains(p.PostId)
							 && !fallbackWeighted.Select(w => w.PostId).Contains(p.PostId))
					.OrderByDescending(p => p.CreatedAt)
					.Take(diversityCount)
					.ToList();

				var combinedFallback = fallbackWeighted
					.Concat(fallbackDiversity)
					.OrderByDescending(p => p.CreatedAt)
					.ToList();

				return combinedFallback.ToListPostResponseDto();
			}

			// 6) Calculăm suma scorurilor pozitive
			double totalScore = positivePrefs.Sum(ucp => ucp.Score);

			// 7) Construim alocarea inițială (floor) pe weightedPostCount
			var allocation = new Dictionary<int, int>(); // { CategoryId -> count }
			int allocatedSum = 0;

			foreach (var ucp in positivePrefs)
			{
				double weight = ucp.Score / totalScore;
				int countForThisCat = (int)Math.Floor(weight * weightedPostCount);
				allocation[ucp.CategoryId] = countForThisCat;
				allocatedSum += countForThisCat;
			}

			// 8) Distribuim restul (remaining) bazat pe fracțiuni
			int remaining = weightedPostCount - allocatedSum;
			var remainders = new List<(int CategoryId, double Remainder)>();

			foreach (var ucp in positivePrefs)
			{
				double exact = (ucp.Score / totalScore) * weightedPostCount;
				double frac = exact - Math.Floor(exact);
				remainders.Add((ucp.CategoryId, frac));
			}

			var sortedByRemainder = remainders
				.OrderByDescending(r => r.Remainder)
				.Select(r => r.CategoryId)
				.ToList();

			for (int i = 0; i < remaining && i < sortedByRemainder.Count; i++)
			{
				int catId = sortedByRemainder[i];
				allocation[catId] = allocation[catId] + 1;
			}

			// 9) Extragem postările ponderate din categoriile pozitive, respectând seenPostIds
			var weightedPosts = new List<Post>();
			foreach (var kv in allocation)
			{
				int categoryId = kv.Key;
				int count = kv.Value;
				if (count <= 0)
					continue;

				var postsForCategory = _postRepository
					.GetAll()
					.Where(p => p.CategoryId == categoryId && !seenPostIds.Contains(p.PostId))
					.OrderByDescending(p => p.CreatedAt)
					.Take(count)
					.ToList();

				weightedPosts.AddRange(postsForCategory);
			}

			// 10) Pregătim lista categoriilor non-preferate pentru diversitate
			var allCategoryIds = _categoryRepository.GetAll().Select(c => c.CategoryId).ToList();
			var preferredCategoryIds = positivePrefs.Select(ucp => ucp.CategoryId).ToList();
			var nonPreferredCategoryIds = allCategoryIds.Except(preferredCategoryIds).ToList();

			// 11) Extragem postările de diversitate (din non-preferate), ne-văzute și neincluse deja
			var diversityPosts = new List<Post>();
			if (nonPreferredCategoryIds.Any() && diversityCount > 0)
			{
				// Luăm din toate postările rămase, filtrate după nonPreferredCategoryIds și seenPostIds
				diversityPosts = _postRepository
					.GetAll()
					.Where(p => nonPreferredCategoryIds.Contains(p.CategoryId)
							 && !seenPostIds.Contains(p.PostId)
							 && !weightedPosts.Select(w => w.PostId).Contains(p.PostId))
					.OrderByDescending(p => p.CreatedAt)
					.Take(diversityCount)
					.ToList();
			}

			// 12) Combinăm weightedPosts + diversityPosts
			var combinedPosts = weightedPosts.Concat(diversityPosts).ToList();

			// 13) Dacă nu am reușit să umplem totalPosts (din cauză că nu erau destule postări disponibile),
			//     completăm cu postări suplimentare din categoria cu cel mai mare scor
			if (combinedPosts.Count < totalPosts)
			{
				int stillNeeded = totalPosts - combinedPosts.Count;

				var topCategoryId = positivePrefs
					.OrderByDescending(ucp => ucp.Score)
					.First().CategoryId;

				var extras = _postRepository
					.GetAll()
					.Where(p => p.CategoryId == topCategoryId
							 && !seenPostIds.Contains(p.PostId)
							 && !combinedPosts.Select(cp => cp.PostId).Contains(p.PostId))
					.OrderByDescending(p => p.CreatedAt)
					.Take(stillNeeded)
					.ToList();

				combinedPosts.AddRange(extras);
			}

			// 14) Sortează întreg feed-ul în funcție de CreatedAt (cronologic descrescător)
			var finalList = combinedPosts
				.OrderByDescending(p => p.CreatedAt)
				.ToList();

			return finalList.ToListPostResponseDto();
		}


	}
}
