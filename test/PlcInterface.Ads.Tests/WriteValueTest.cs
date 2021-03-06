﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using PlcInterface.Ads.Tests.DataTypes;
using PlcInterface.Tests;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Threading.Tasks;
using TwinCAT.Ads;

namespace PlcInterface.Ads.Tests
{
    [TestClass]
    public class WriteValueTest : IWriteValueTestBase
    {
        private static PlcConnection connection = null;
        private static ReadWrite readWrite = null;
        private static SymbolHandler symbolHandler = null;

        protected override string BoolValue
            => (string)Settings.GetWriteData().First()[0];

        protected override IEnumerable<object[]> Data
            => Settings.GetWriteData();

        [ClassInitialize]
        public static async Task ConnectAsync(TestContext testContext)
        {
            var connectionsettings = new ConnectionSettings() { AmsNetId = Settings.AmsNetId, Port = Settings.Port };
            var symbolhandlersettings = new SymbolHandlerSettings() { StoreSymbolsToDisk = false };
            var dynamicValueConverter = new DynamicValueConverter();

            connection = new PlcConnection(GetOptionsMoq(connectionsettings), GetLoggerMock<PlcConnection>());
            symbolHandler = new SymbolHandler(connection, GetOptionsMoq(symbolhandlersettings), GetLoggerMock<SymbolHandler>());
            readWrite = new ReadWrite(connection, symbolHandler, dynamicValueConverter, GetLoggerMock<ReadWrite>());
            await connection.ConnectAsync();
            var result = await connection.SessionStream.FirstAsync();
        }

        [ClassCleanup]
        public static void Disconnect()
            => connection.Dispose();

        [TestCleanup]
        public async Task ResetDataAsync()
        {
            var client = await connection.SessionStream.Cast<IConnected<TcAdsClient>>().FirstAsync();
            var errorCode = client.Value.TryWriteControl(new StateInfo(AdsState.Reset, 0));
            Assert.AreEqual(AdsErrorCode.NoError, errorCode);
            errorCode = client.Value.TryWriteControl(new StateInfo(AdsState.Run, 0));
            Assert.AreEqual(AdsErrorCode.NoError, errorCode);
        }

        [TestMethod]
        public override void WriteGeneric()
        {
            WriteValueGenericHelper("WriteTestData.BoolValue", false);
            WriteValueGenericHelper("WriteTestData.ByteValue", byte.MinValue);
            WriteValueGenericHelper("WriteTestData.WordValue", ushort.MinValue);
            WriteValueGenericHelper("WriteTestData.DWordValue", uint.MinValue);
            WriteValueGenericHelper("WriteTestData.LWordValue", ulong.MinValue);
            WriteValueGenericHelper("WriteTestData.ShortValue", sbyte.MaxValue);
            WriteValueGenericHelper("WriteTestData.IntValue", short.MaxValue);
            WriteValueGenericHelper("WriteTestData.DIntValue", int.MaxValue);
            WriteValueGenericHelper("WriteTestData.LongValue", long.MaxValue);
            WriteValueGenericHelper("WriteTestData.UShortValue", byte.MinValue);
            WriteValueGenericHelper("WriteTestData.UIntValue", ushort.MinValue);
            WriteValueGenericHelper("WriteTestData.UDIntValue", uint.MinValue);
            WriteValueGenericHelper("WriteTestData.ULongValue", ulong.MinValue);
            WriteValueGenericHelper("WriteTestData.FloatValue", float.MaxValue);
            WriteValueGenericHelper("WriteTestData.DoubleValue", double.MaxValue);
            WriteValueGenericHelper("WriteTestData.TimeValue", TimeSpan.FromMilliseconds(3000));
            WriteValueGenericHelper("WriteTestData.TimeOfDay", TimeSpan.FromHours(10));
            WriteValueGenericHelper("WriteTestData.LTimeValue", TimeSpan.FromTicks(100));
            WriteValueGenericHelper("WriteTestData.DateValue", new DateTime(2019, 02, 22));
            WriteValueGenericHelper("WriteTestData.DateAndTimeValue", new DateTime(2019, 02, 22, 12, 15, 10));
            WriteValueGenericHelper("WriteTestData.StringValue", "new Test String");
            WriteValueGenericHelper("WriteTestData.WStringValue", "new Test WString");
            WriteValueGenericHelper("WriteTestData.EnumValue", (short)TestEnum.third);
            WriteValueGenericHelper("WriteTestData.IntArray", new short[] { 10000, 10001, 10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010 });
        }

        [TestMethod]
        public override async Task WriteGenericAsync()
        {
            await WriteValueGenericHelperAsync("WriteTestData.BoolValue", false);
            await WriteValueGenericHelperAsync("WriteTestData.ByteValue", byte.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.WordValue", ushort.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.DWordValue", uint.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.LWordValue", ulong.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.ShortValue", sbyte.MaxValue);
            await WriteValueGenericHelperAsync("WriteTestData.IntValue", short.MaxValue);
            await WriteValueGenericHelperAsync("WriteTestData.DIntValue", int.MaxValue);
            await WriteValueGenericHelperAsync("WriteTestData.LongValue", long.MaxValue);
            await WriteValueGenericHelperAsync("WriteTestData.UShortValue", byte.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.UIntValue", ushort.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.UDIntValue", uint.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.ULongValue", ulong.MinValue);
            await WriteValueGenericHelperAsync("WriteTestData.FloatValue", float.MaxValue);
            await WriteValueGenericHelperAsync("WriteTestData.DoubleValue", double.MaxValue);
            await WriteValueGenericHelperAsync("WriteTestData.TimeValue", TimeSpan.FromMilliseconds(3000));
            await WriteValueGenericHelperAsync("WriteTestData.TimeOfDay", TimeSpan.FromHours(10));
            await WriteValueGenericHelperAsync("WriteTestData.LTimeValue", TimeSpan.FromTicks(100));
            await WriteValueGenericHelperAsync("WriteTestData.DateValue", new DateTime(2019, 02, 22));
            await WriteValueGenericHelperAsync("WriteTestData.DateAndTimeValue", new DateTime(2019, 02, 22, 12, 15, 10));
            await WriteValueGenericHelperAsync("WriteTestData.StringValue", "new Test String");
            await WriteValueGenericHelperAsync("WriteTestData.WStringValue", "new Test WString");
            await WriteValueGenericHelperAsync("WriteTestData.EnumValue", (short)TestEnum.third);
            await WriteValueGenericHelperAsync("WriteTestData.IntArray", new short[] { 10000, 10001, 10002, 10003, 10004, 10005, 10006, 10007, 10008, 10009, 10010 });
        }

        protected override IPlcConnection GetPLCConnection()
            => connection;

        protected override IReadWrite GetReadWrite()
            => readWrite;

        protected override ISymbolHandler GetSymbolHandler()
            => symbolHandler;
    }
}