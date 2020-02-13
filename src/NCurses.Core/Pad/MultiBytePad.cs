﻿using System;
using System.Collections.Generic;
using System.Text;

using NCurses.Core.Window;
using NCurses.Core.Interop;
using NCurses.Core.Interop.MultiByte;
using NCurses.Core.Interop.SingleByte;
using NCurses.Core.Interop.WideChar;
using NCurses.Core.Interop.Char;
using NCurses.Core.Interop.Mouse;
using NCurses.Core.Interop.SafeHandles;
using NCurses.Core.Interop.Wrappers;

namespace NCurses.Core.Pad
{
    public class MultiBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> : PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>
        where TMultiByte : unmanaged, IMultiByteNCursesChar, IEquatable<TMultiByte>
        where TWideChar : unmanaged, IMultiByteChar, IEquatable<TWideChar>
        where TSingleByte : unmanaged, ISingleByteNCursesChar, IEquatable<TSingleByte>
        where TChar : unmanaged, ISingleByteChar, IEquatable<TChar>
        where TMouseEvent : unmanaged, IMEVENT
    {
        public override bool HasUnicodeSupport => true;

        internal MultiBytePad(WindowBaseSafeHandle windowBaseSafeHandle)
            : base(windowBaseSafeHandle)
        { }

        internal MultiBytePad(
            WindowBaseSafeHandle windowBaseSafeHandle,
            PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentPad)
            : base(windowBaseSafeHandle, parentPad)
        { }

        internal MultiBytePad(WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> window)
            : base(window)
        { }

        public MultiBytePad(int nlines, int ncols)
            : base(NCurses.newpad(nlines, ncols))
        { }

        public override void Echo(char ch)
        {
            TMultiByte wCh = MultiByteCharFactoryInternal<TMultiByte>.Instance.GetNativeCharInternal(ch);
            Pad.pecho_wchar(this.WindowBaseSafeHandle, in wCh);
        }

        public override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> Duplicate()
        {
            return new MultiBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(
                new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(
                    NCurses.dupwin(this.WindowBaseSafeHandle)));
        }

        internal override PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreatePad(
            WindowBaseSafeHandle windowBaseSafeHandle,
            PadBase<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentPad)
        {
            return new MultiBytePad<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentPad);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowBaseSafeHandle windowBaseSafeHandle,
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> parentWindow)
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(windowBaseSafeHandle, parentWindow);
        }

        internal override WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> CreateWindow(
            WindowInternal<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent> existingWindow)
        {
            return new MultiByteWindow<TMultiByte, TWideChar, TSingleByte, TChar, TMouseEvent>(existingWindow);
        }
    }
}