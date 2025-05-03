using Forum.Domain.Entities;

namespace Forum.Application.Shared.Comparators;

//TODO: от этого надо избавляться. Может, поле Order и сортировка по нему?
public class AnswerComparator : IComparer<Answer>
{
    public int Compare(Answer? x, Answer? y)
    {
        if (x is null && y is null) 
            return 0;
        
        if (x is null)
            return 1;

        if (y is null)
            return -1;

        if (x.ParentId is null && y.ParentId is not null)
            return -1;

        if (y.ParentId is null && x.ParentId is not null)
            return 1;

        return x.CreatedAt.CompareTo(y.CreatedAt);
    }
}
