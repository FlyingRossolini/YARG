using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace GardenMVC.Common_Types
{
    public enum SequentialGuidType
    {
        SequentialAsString,
        SequentialAsBinary,
        SequentialAtEnd
    }


    public class Global
    {
        private static readonly RNGCryptoServiceProvider _rng = new RNGCryptoServiceProvider();

        public static Guid NewSequentialGuid(SequentialGuidType guidType)
        {
            byte[] randomBytes = new byte[10];
            _rng.GetBytes(randomBytes);

            long timestamp = DateTime.UtcNow.Ticks / 10000L;
            byte[] timestampBytes = BitConverter.GetBytes(timestamp);

            if (BitConverter.IsLittleEndian)
            {
                Array.Reverse(timestampBytes);
            }

            byte[] guidBytes = new byte[16];

            switch (guidType)
            {
                case SequentialGuidType.SequentialAsString:
                case SequentialGuidType.SequentialAsBinary:
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 0, 6);
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 6, 10);

                    // If formatting as a string, we have to reverse the order
                    // of the Data1 and Data2 blocks on little-endian systems.
                    if (guidType == SequentialGuidType.SequentialAsString && BitConverter.IsLittleEndian)
                    {
                        Array.Reverse(guidBytes, 0, 4);
                        Array.Reverse(guidBytes, 4, 2);
                    }
                    break;

                case SequentialGuidType.SequentialAtEnd:
                    Buffer.BlockCopy(randomBytes, 0, guidBytes, 0, 10);
                    Buffer.BlockCopy(timestampBytes, 2, guidBytes, 10, 6);
                    break;
            }

            return new Guid(guidBytes);
        }
    
        
    }

    public class Constants
    {
        public static readonly Guid LimitType_LCL = new Guid("3a03a0f5-27e6-d4a6-a3e7-ef1f0877283f");
        public static readonly Guid LimitType_UCL = new Guid("3a03a0f4-fe17-340e-a311-f553d5cef030");
        public static readonly Guid MeasurementType_TempDay = new Guid("3a03ad68-02a1-3c15-aaae-e2fa08e24c8d");
        public static readonly Guid MeasurementType_TempNight = new Guid("3a03ad68-2b58-1a34-f4ef-f7e4bfc57587");
        public static readonly Guid MeasurementType_Humidity = new Guid("3a030472-0d80-2519-8007-5a74813c8bba");
        public static readonly Guid MeasurementType_pH = new Guid("3a0307d4-48b7-9a1d-b283-855ddb37b3d3");
        public static readonly Guid MeasurementType_PPM = new Guid("3a0307d4-7913-8e12-b823-4aa5ccc8d100");
        public static readonly Guid LocationType_Habitat = new Guid("3a030487-3e43-0455-fe56-125537762d27");
        public static readonly Guid LocationType_Reservoir = new Guid("3a030486-4c6e-174a-5cda-f6eb18dbb247");

    }
}
