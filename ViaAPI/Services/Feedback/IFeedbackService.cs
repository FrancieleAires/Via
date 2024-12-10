using ViaAPI.Helpers;
using ViaAPI.Models;
namespace ViaAPI.Services.FeedbackService
{
    public interface IFeedbackService
    {
        Task<Result> CreateFeedbackAsync(FeedbackViewModel feedbackViewModel, Guid userId);
        Task<FeedbackModel> GetFeedbackAsync(FeedbackModel feedbackModel);
        Task<List<FeedbackModel>> HistoryFeedback(Guid userId);

    }
}
