﻿using System;
using System.Collections.Generic;
using System.Text;

namespace NCurses.Core.Interop
{
    public interface ICharFactory<TChar, TString>
        where TChar : IChar
        where TString : ICharString
    {
        TChar GetNativeEmptyChar();
        TChar GetNativeChar(char ch);

        TString GetNativeEmptyString(int length);
        TString GetNativeString(string str);
        TString GetNativeString(ReadOnlySpan<char> str);

        unsafe TString GetNativeEmptyString(byte* buffer, int bufferLength, int stringLength);
        TString GetNativeEmptyString(byte[] buffer, int bufferLength, int stringLength);

        unsafe TString GetNativeString(byte* buffer, int bufferLength, string str);
        unsafe TString GetNativeString(byte* buffer, int bufferLength, ReadOnlySpan<char> str);
        TString GetNativeString(byte[] buffer, int bufferLength, string str);
        TString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str);

        int GetByteCount(string str);
        int GetByteCount(ReadOnlySpan<char> str);
        int GetByteCount(int length);
        int GetCharLength();
    }

    public interface INCursesCharFactory<TChar, TString> : ICharFactory<TChar, TString>
        where TChar : INCursesChar
        where TString : INCursesCharString
    {
        TChar GetNativeChar(char ch, ulong attrs);
        TChar GetNativeChar(char ch, ulong attrs, short colorPair);

        TString GetNativeString(string str, ulong attrs);
        TString GetNativeString(ReadOnlySpan<char> str, ulong attrs);
        TString GetNativeString(string str, ulong attrs, short colorPair);
        TString GetNativeString(ReadOnlySpan<char> str, ulong attrs, short colorPair);

        unsafe TString GetNativeString(byte* buffer, int bufferLength, string str, ulong attrs);
        unsafe TString GetNativeString(byte* buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs);
        TString GetNativeString(byte[] buffer, int bufferLength, string str, ulong attrs);
        TString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs);
        unsafe TString GetNativeString(byte* buffer, int bufferLength, string str, ulong attrs, short colorPair);
        unsafe TString GetNativeString(byte* buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs, short colorPair);
        TString GetNativeString(byte[] buffer, int bufferLength, string str, ulong attrs, short colorPair);
        TString GetNativeString(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs, short colorPair);
    }

    internal interface ICharFactoryInternal<TChar, TString>
        where TChar : unmanaged, IChar, IEquatable<TChar>
        where TString : struct, ICharString
    {
        TChar GetNativeEmptyCharInternal();
        TChar GetNativeCharInternal(char ch);

        unsafe TString GetNativeEmptyStringInternal(byte* buffer, int bufferLength, int stringLength);
        TString GetNativeEmptyStringInternal(byte[] buffer, int bufferLength, int stringLength);
        unsafe TString GetNativeStringInternal(byte* buffer, int bufferLength, string str);
        unsafe TString GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySpan<char> str);
        TString GetNativeStringInternal(byte[] buffer, int bufferLength, string str);
        TString GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<char> str);

        TString CreateNativeString(ref TChar strRef);

        int GetByteCount(string str, bool addNullTerminator = true);
        int GetByteCount(ReadOnlySpan<char> str, bool addNullTerminator = true);
        int GetByteCount(int length, bool addNullTerminator = true);
        int GetCharLength();
    }

    internal interface INCursesCharFactoryInternal<TCharType, TStringType, TChar, TString> : 
        ICharFactoryInternal<TChar, TString>, INCursesCharFactory<TCharType, TStringType>
        where TCharType : INCursesChar
        where TStringType : INCursesCharString, IEnumerable<TCharType>
        where TChar : unmanaged, TCharType, IEquatable<TChar>, IEquatable<TCharType>
        where TString : struct, TStringType, IEnumerable<TCharType>
    {
        TChar GetNativeCharInternal(char ch, ulong attrs);
        TChar GetNativeCharInternal(char ch, ulong attrs, short colorPair);

        unsafe TString GetNativeStringInternal(byte* buffer, int bufferLength, string str, ulong attrs);
        unsafe TString GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs);
        TString GetNativeStringInternal(byte[] buffer, int bufferLength, string str, ulong attrs);
        TString GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs);
        unsafe TString GetNativeStringInternal(byte* buffer, int bufferLength, string str, ulong attrs, short colorPair);
        unsafe TString GetNativeStringInternal(byte* buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs, short colorPair);
        TString GetNativeStringInternal(byte[] buffer, int bufferLength, string str, ulong attrs, short colorPair);
        TString GetNativeStringInternal(byte[] buffer, int bufferLength, ReadOnlySpan<char> str, ulong attrs, short colorPair);
    }
}
