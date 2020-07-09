using System;
using System.Buffers;
using System.Collections.Generic;
using System.Text;

namespace THNETII.InteropServices.Memory
{
    public static class MarshalAllocator
    {
        public static CoTaskMemAllocator CoTaskMem { get; } =
            CoTaskMemAllocator.Instance;
    }

    public class CoTaskMemAllocator : IAllocator
    {
        public static CoTaskMemAllocator Instance { get; } =
            new CoTaskMemAllocator();
    }

    public interface IAllocator
    {
    }
}
