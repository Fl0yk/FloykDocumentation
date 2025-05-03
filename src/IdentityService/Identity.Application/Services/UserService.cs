namespace Identity.Application.Services;

//TODO: thinking about saved articles
public class UserService
{

    //public async Task SaveArticleAsync(SaveArticleRequest articleRequest, CancellationToken cancellationToken = default)
    //{
    //    var user = _currentUserProvider.GetCurrentUser();

    //    if (user is null)
    //    {
    //        throw new GuardUnauthorizedException("User is not authorize");
    //    }

    //    var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

    //    if (dbUser is null)
    //    {
    //        throw new GuardNotFoundException($"User with id {user.Id} was not found");
    //    }

    //    if (dbUser.SavedArticles.Any(a => a.ArticleId == articleRequest.Id))
    //    {
    //        throw new GuardArgumentException("User already save this article");
    //    }

    //    bool isExist = await _articleService.IsArticleExistAsync(articleRequest.Id, cancellationToken);

    //    if (!isExist)
    //    {
    //        throw new GuardNotFoundException($"Article with id {articleRequest.Id} was not found");
    //    }

    //    Article article = await _articleService.GetArticleByIdAsync(articleRequest.Id, cancellationToken);

    //    dbUser.SavedArticles.Add(_mapper.Map<SavedArticle>(article));

    //    await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

    //    await _unitOfWork.SaveChangesAsync(cancellationToken);
    //}

    //public async Task RemoveSavedArticleAsync(Guid articleId, CancellationToken cancellationToken = default)
    //{
    //    var user = _currentUserProvider.GetCurrentUser();

    //    if (user is null)
    //    {
    //        throw new GuardUnauthorizedException("User is not authorize");
    //    }

    //    var dbUser = await _unitOfWork.UserRepository.GetUserByIdAsync(user.Id, cancellationToken);

    //    if (dbUser is null)
    //    {
    //        throw new GuardNotFoundException($"User with id {user.Id} was not found");
    //    }

    //    var savedArticle = dbUser.SavedArticles.FirstOrDefault(a => a.ArticleId == articleId);

    //    if (savedArticle is null)
    //    {
    //        throw new BadRequestException($"User don't have saved article with id {articleId}");
    //    }

    //    dbUser.SavedArticles.Remove(savedArticle);

    //    await _unitOfWork.UserRepository.UpdateAsync(dbUser, cancellationToken);

    //    await _unitOfWork.SaveChangesAsync(cancellationToken);
    //}

    //public async Task RemoveSavedArticleForAllAsync(Guid articleId, CancellationToken cancellationToken = default)
    //{
    //    await _unitOfWork.UserRepository.DeleteSavedArticleForAllAsync(articleId, cancellationToken);

    //    await _unitOfWork.SaveChangesAsync(cancellationToken);
    //}
}
