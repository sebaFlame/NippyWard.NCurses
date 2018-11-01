﻿using System;
using System.Collections.Generic;
using System.Text;
using NCurses.Core.Interop.Small;
using NCurses.Core.Interop.WideStr;

namespace NCurses.Core.Interop.Wide
{
    public interface INativeScreenWide
    {
        void wunctrl_sp(IntPtr screen, in INCursesWCHAR wch, out string str);
    }

    public class NativeScreenWide<TWide, TWideStr, TSmall, TSmallStr> : NativeWideBase<TWide, TWideStr, TSmall, TSmallStr>, INativeScreenWide
        where TWide : unmanaged, INCursesWCHAR
        where TWideStr : unmanaged
        where TSmall : unmanaged, INCursesSCHAR
        where TSmallStr : unmanaged
    {
        public void wunctrl_sp(IntPtr screen, in INCursesWCHAR wch, out string str)
        {
            str = NativeWideStrBase<TWideStr, TSmallStr>.ReadString(ref this.Wrapper.wunctrl_sp(screen, MarshallArrayReadonly(wch)));
        }
    }
}