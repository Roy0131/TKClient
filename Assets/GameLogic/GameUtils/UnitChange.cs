public class UnitChange
{
    /// <summary>
    ///  单位转换 向下取整
    /// </summary>
    /// <param name="Num"></param>
    /// <returns></returns>
    public static string GetUnitNum(int Num)
    {
        int mNum = 1000 * 1000;
        int kNum = 1000;
        if (Num / mNum >= 10)
            return Num / mNum + "M";
        else if (Num / kNum >= 10)
            return Num / kNum + "K";
        else
            return Num.ToString();
    }
}
