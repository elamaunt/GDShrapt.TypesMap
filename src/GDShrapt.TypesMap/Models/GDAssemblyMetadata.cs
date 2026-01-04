namespace GDShrapt.TypesMap
{
    /// <summary>
    /// Contains metadata about the extracted assembly data.
    /// Used for versioning and tracking data source information.
    /// </summary>
    public class GDAssemblyMetadata
    {
        /// <summary>
        /// Godot engine version (e.g., "4.5.1").
        /// </summary>
        public string? GodotVersion { get; set; }

        /// <summary>
        /// Data format version for compatibility checking.
        /// Increment when making breaking changes to the data structure.
        /// </summary>
        public int DataFormatVersion { get; set; } = 1;

        /// <summary>
        /// UTC timestamp when the data was extracted.
        /// </summary>
        public DateTime? ExtractedAt { get; set; }

        /// <summary>
        /// Data source identifier: "Assembly", "File", or "Manifest".
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// File path when source is "File".
        /// </summary>
        public string? SourcePath { get; set; }

        /// <summary>
        /// Creates a new instance with default values.
        /// </summary>
        public GDAssemblyMetadata()
        {
        }

        /// <summary>
        /// Creates a new instance for assembly extraction.
        /// </summary>
        /// <param name="godotVersion">Godot engine version string.</param>
        public GDAssemblyMetadata(string godotVersion)
        {
            GodotVersion = godotVersion;
            DataFormatVersion = 1;
            ExtractedAt = DateTime.UtcNow;
            Source = "Assembly";
        }
    }
}
