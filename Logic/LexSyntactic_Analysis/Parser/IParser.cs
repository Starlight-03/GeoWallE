using System.Collections.Generic;

public interface IParser
{
    public List<string> Errors { get; }

    public GeoWallE_Program Program { get; }
}