namespace Fenix
{
    /// <summary>
    /// Popisuje mod aplikace
    /// </summary>
    internal enum ApplicationMode
    {
        /// <summary>
        /// Mod, kdy je aplikace spuštěna
        /// </summary>
        Run = 1,
        /// <summary>
        /// Mod, kdy je zakázána prováděcí část
        /// </summary>
        ExecutiveProhibited = 2,
        /// <summary>
        /// Mod, kdy je aplikace v režimu self test
        /// </summary>
        SelfTest = 3
    }
}