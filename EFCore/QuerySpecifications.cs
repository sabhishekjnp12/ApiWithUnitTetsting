//using System.Data.Entity;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Linq.Expressions;

namespace ApiWithUnitTetsting.EFCore;

public class QuerySpecification<T> : IQuerySpecification<T> where T : class
{
    #region Private Fields

    private bool _isCustomFilterApplied = false;

    private bool _isCustomSortApplied = false;

    private bool _isGroupApplied = false;

    #endregion

    #region Public Constructors

    public QuerySpecification()
    {
    }

    public QuerySpecification(Func<IQueryable<T>, IOrderedQueryable<T>> defaultSort)
    {
        DefaultSort = defaultSort;
    }

    public QuerySpecification(Expression<Func<T, bool>> criteria, Func<IQueryable<T>, IOrderedQueryable<T>>? defaultSort)
    {
        Criteria = criteria;
        DefaultSort = defaultSort;
    }

    #endregion

    #region Public Properties

    public Expression<Func<T, bool>>? Criteria { get; set; }

    public Func<IQueryable<T>, IOrderedQueryable<T>>? CurrentSort
    {
        get
        {
            if (IsCustomSortApplied)
                return CustomSort;
            else
                return DefaultSort;
        }
    }

    public Expression<Func<T, bool>>? CustomFilter { get; set; }

    public Expression<Func<T, bool>>? CustomSearch { get; set; }

    public Func<IQueryable<T>, IOrderedQueryable<T>>? CustomSort { get; set; }

    public Func<IQueryable<T>, IOrderedQueryable<T>>? DefaultSort { get; }

    public Expression<Func<T, object>>? GroupExpression { get; set; }

    public HashSet<Expression<Func<T, object>>>? IncludeExpressions { get; private set; }

    public HashSet<string>? IncludeStrings { get; private set; }

    public bool IsCustomFilterApplied
    {
        get => _isCustomFilterApplied;
        set
        {
            if (value && CustomFilter == null)
                throw new InvalidOperationException();
            _isCustomFilterApplied = value;
        }
    }

    public bool IsCustomSortApplied
    {
        get => _isCustomSortApplied;
        set
        {
            if (value && CustomSort == null)
                throw new InvalidOperationException();
            _isCustomSortApplied = value;
        }
    }

    public bool IsGroupApplied
    {
        get => _isGroupApplied;
        set
        {
            if (value && GroupExpression == null)
                throw new InvalidOperationException();
            _isGroupApplied = value;
        }
    }

    #endregion

    #region Public Methods

    public virtual void AddIncludeExpression(Expression<Func<T, object>> includeExpression)
    {
        if (IncludeExpressions == null)
            IncludeExpressions = new HashSet<Expression<Func<T, object>>>();
        IncludeExpressions.Add(includeExpression);
    }

    public virtual void AddIncludeString(string includeString)
    {
        if (IncludeStrings == null)
            IncludeStrings = new HashSet<string>();
        IncludeStrings.Add(includeString);
    }

    public IQueryable<T> GetQuery(IQueryable<T> inputQuery, bool? sorted, bool? grouped, bool searchmatches)
    {
        Debug.WriteLine("GetQuery");
        IQueryable<T> result = inputQuery;//.AsSplitQuery();// TODO
        if (Criteria != null)
            result = result.Where(Criteria);
        if (CustomFilter != null && IsCustomFilterApplied)
            result = result.Where(CustomFilter);
        if (searchmatches)
        {
            if (CustomSearch == null)
                throw new InvalidOperationException();
            else
            {
                Debug.WriteLine("Start result = result.Where(CustomSearch);");
                result = result.Where(CustomSearch);
                Debug.WriteLine("End result = result.Where(CustomSearch);");
            }
        }
        if (IncludeExpressions != null && IncludeExpressions.Any())
        {
            Debug.WriteLine("Include Added to query");
            result = IncludeExpressions.Aggregate(result, (current, include) => current.Include(include));
        }

        if (IncludeStrings != null && IncludeStrings.Any())
            result = IncludeStrings.Aggregate(result, (current, include) => current.Include(include));
        if (!sorted.HasValue)
        {
            if (CurrentSort != null)
                result = CurrentSort(result);
        }
        else if (sorted.Value)
        {
            if (CurrentSort == null)
                throw new InvalidOperationException();
            else
                result = CurrentSort(result);
        }
        if (!grouped.HasValue)
        {
            if (GroupExpression != null)
                result = result.GroupBy(GroupExpression).SelectMany(x => x);
        }
        else if (grouped.Value)
        {
            if (GroupExpression == null)
                throw new InvalidOperationException();
            else
                result = result.GroupBy(GroupExpression).SelectMany(x => x);
        }
        Debug.WriteLine("return result");
        return result;
    }

    public IQueryable<T> GetQuery(Microsoft.EntityFrameworkCore.ChangeTracking.LocalView<T> inputQuery, bool? sorted, bool? grouped, bool searchmatches)
    {
        IQueryable<T> result = inputQuery.AsQueryable();//.AsSplitQuery();// TODO
        if (Criteria != null)
            result = result.Where(Criteria);
        if (CustomFilter != null && IsCustomFilterApplied)
            result = result.Where(CustomFilter);
        if (searchmatches)
        {
            if (CustomSearch == null)
                throw new InvalidOperationException();
            else
                result = result.Where(CustomSearch);
        }
        if (IncludeExpressions != null && IncludeExpressions.Any())
        {
            Debug.WriteLine("Include Added to query");
            result = IncludeExpressions.Aggregate(result, (current, include) => current.Include(include));
        }

        if (IncludeStrings != null && IncludeStrings.Any())
            result = IncludeStrings.Aggregate(result, (current, include) => current.Include(include));
        if (!sorted.HasValue)
        {
            if (CurrentSort != null)
                result = CurrentSort(result);
        }
        else if (sorted.Value)
        {
            if (CurrentSort == null)
                throw new InvalidOperationException();
            else
                result = CurrentSort(result);
        }
        if (!grouped.HasValue)
        {
            if (GroupExpression != null)
                result = result.GroupBy(GroupExpression).SelectMany(x => x);
        }
        else if (grouped.Value)
        {
            if (GroupExpression == null)
                throw new InvalidOperationException();
            else
                result = result.GroupBy(GroupExpression).SelectMany(x => x);
        }
        return result;
    }

    #endregion

    //public int Take { get; private set; }
    //public int Skip { get; private set; }
    //public bool IsPagingEnabled { get; private set; } = false;

    //public virtual void ApplyPaging(int skip, int take)
    //{
    //    Skip = skip;
    //    Take = take;
    //    IsPagingEnabled = true;
    //}
}