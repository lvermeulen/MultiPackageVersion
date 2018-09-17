namespace MultiPackageVersion.Core
{
    public enum DifferenceType
    {
        Untracked,
        Ignored,
        Unmodified,
        Modified,
        Added,
        Deleted,
        Renamed,
        Copied,
        UpdatedButUnmerged
    }
}