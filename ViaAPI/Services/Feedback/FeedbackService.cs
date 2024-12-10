using Microsoft.EntityFrameworkCore;
using ViaAPI.Data;
using ViaAPI.Helpers;
using ViaAPI.Models;

namespace ViaAPI.Services.FeedbackService
{
    public class FeedbackService : IFeedbackService
    {
        private readonly ApiDbContext _dbContext;


        public FeedbackService(ApiDbContext dbContext)
        {
            _dbContext = dbContext;

        }

        public async Task<Result> CreateFeedbackAsync(FeedbackViewModel feedbackViewModel, Guid userId)
        {
            if (feedbackViewModel == null || string.IsNullOrWhiteSpace(feedbackViewModel.Comentario))
            {
                return Result.Failure("Feedback inválido.");
            }
            var feedbackModel = new FeedbackModel
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Comentario = feedbackViewModel.Comentario,
                Avaliacao = feedbackViewModel.Nota,
                Data = DateTime.UtcNow
            };

            await _dbContext.Feedback.AddAsync(feedbackModel);

            var saveResult = await _dbContext.SaveChangesAsync();

            if (saveResult > 0)
            {
                return Result.Success("Feedback criado com sucesso.");
            }
            else
            {
                return Result.Failure("Erro ao criar feedback: operação não foi bem-sucedida.");
            }
        }

        public async Task<FeedbackModel> GetFeedbackAsync(FeedbackModel feedbackModel)
        {
            if (feedbackModel == null)
            {
                return null;
            }

            var feedback = await _dbContext.Feedback
                .FirstOrDefaultAsync(f => f.Id == feedbackModel.Id);

            if (feedback == null)
            {
                return null;
            }

            return feedback;
        }

        public async Task<List<FeedbackModel>> HistoryFeedback(Guid userId)
        {
            var feedbacks = await _dbContext.Feedback
        .Where(f => f.UserId == userId)
        .ToListAsync();

            if (feedbacks == null || !feedbacks.Any())
            {
                return null;
            }


            return feedbacks;
        }
    }
}
