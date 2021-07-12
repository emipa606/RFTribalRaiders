namespace TribalRaiders_Code
{
    public static class BackstoryDefExt
    {
        public static string UniqueSaveKey(this BackstoryDef def)
        {
            return "TR_" + def.defName;
        }
    }
}