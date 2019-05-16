namespace IHLogic
{
    public delegate void REventDelegate();
    public delegate void REventDelegate<T>(T a);
    public delegate void REventDelegate<T1, T2>(T1 a1, T2 a2);
    public delegate void REventDelegate<T1, T2, T3>(T1 a1, T2 a2, T3 a3);
    public delegate void REventDelegate<T1, T2, T3, T4>(T1 a1, T2 a2, T3 a3, T4 a4);
}
