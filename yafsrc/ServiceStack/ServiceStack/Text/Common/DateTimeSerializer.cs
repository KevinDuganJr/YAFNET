﻿// ***********************************************************************
// <copyright file="DateTimeSerializer.cs" company="ServiceStack, Inc.">
//     Copyright (c) ServiceStack, Inc. All Rights Reserved.
// </copyright>
// <summary>Fork for YetAnotherForum.NET, Licensed under the Apache License, Version 2.0</summary>
// ***********************************************************************

using System;
using System.Globalization;
using System.IO;

using ServiceStack.Text.Json;
using ServiceStack.Text.Support;
using System.Text.RegularExpressions;

namespace ServiceStack.Text.Common
{
    /// <summary>
    /// Class DateTimeSerializer.
    /// </summary>
    public static class DateTimeSerializer
    {
        /// <summary>
        /// The condensed date time format
        /// </summary>
        public const string CondensedDateTimeFormat = "yyyyMMdd";                             //8
        /// <summary>
        /// The short date time format
        /// </summary>
        public const string ShortDateTimeFormat = "yyyy-MM-dd";                               //11
        /// <summary>
        /// The default date time format
        /// </summary>
        public const string DefaultDateTimeFormat = "dd/MM/yyyy HH:mm:ss";                    //20
        /// <summary>
        /// The default date time format with fraction
        /// </summary>
        public const string DefaultDateTimeFormatWithFraction = "dd/MM/yyyy HH:mm:ss.fff";    //24
        /// <summary>
        /// The XSD date time format
        /// </summary>
        public const string XsdDateTimeFormat = "yyyy-MM-ddTHH:mm:ss.fffffffZ";               //29
        /// <summary>
        /// The XSD date time format3 f
        /// </summary>
        public const string XsdDateTimeFormat3F = "yyyy-MM-ddTHH:mm:ss.fffZ";                 //25
        /// <summary>
        /// The XSD date time format seconds
        /// </summary>
        public const string XsdDateTimeFormatSeconds = "yyyy-MM-ddTHH:mm:ssZ";                //21
        /// <summary>
        /// The date time format seconds UTC offset
        /// </summary>
        public const string DateTimeFormatSecondsUtcOffset = "yyyy-MM-ddTHH:mm:sszzz";        //22
        /// <summary>
        /// The date time format seconds no offset
        /// </summary>
        public const string DateTimeFormatSecondsNoOffset = "yyyy-MM-ddTHH:mm:ss";
        /// <summary>
        /// The date time format ticks UTC offset
        /// </summary>
        public const string DateTimeFormatTicksUtcOffset = "yyyy-MM-ddTHH:mm:ss.fffffffzzz";  //30
        /// <summary>
        /// The date time format ticks no UTC offset
        /// </summary>
        public const string DateTimeFormatTicksNoUtcOffset = "yyyy-MM-ddTHH:mm:ss.fffffff";

        /// <summary>
        /// The escaped WCF json prefix
        /// </summary>
        public const string EscapedWcfJsonPrefix = "\\/Date(";
        /// <summary>
        /// The escaped WCF json suffix
        /// </summary>
        public const string EscapedWcfJsonSuffix = ")\\/";
        /// <summary>
        /// The WCF json prefix
        /// </summary>
        public const string WcfJsonPrefix = "/Date(";
        /// <summary>
        /// The WCF json suffix
        /// </summary>
        public const char WcfJsonSuffix = ')';
        /// <summary>
        /// The unspecified offset
        /// </summary>
        public const string UnspecifiedOffset = "-0000";
        /// <summary>
        /// The UTC offset
        /// </summary>
        public const string UtcOffset = "+0000";

        /// <summary>
        /// The XSD time separator
        /// </summary>
        private const char XsdTimeSeparator = 'T';
        /// <summary>
        /// The XSD time separator index
        /// </summary>
        private static readonly int XsdTimeSeparatorIndex = XsdDateTimeFormat.IndexOf(XsdTimeSeparator);
        /// <summary>
        /// The XSD UTC suffix
        /// </summary>
        private const string XsdUtcSuffix = "Z";
        /// <summary>
        /// The date time separators
        /// </summary>
        private static readonly char[] DateTimeSeparators = { '-', '/' };
        /// <summary>
        /// The UTC offset information regex
        /// </summary>
        private static readonly Regex UtcOffsetInfoRegex = new("([+-](?:2[0-3]|[0-1][0-9]):[0-5][0-9])", PclExport.Instance.RegexOptions);
        /// <summary>
        /// Gets or sets the on parse error function.
        /// </summary>
        /// <value>The on parse error function.</value>
        public static Func<string, Exception, DateTime> OnParseErrorFn { get; set; }

        /// <summary>
        /// If AlwaysUseUtc is set to true then convert all DateTime to UTC. If PreserveUtc is set to true then UTC dates will not convert to local
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <param name="parsedAsUtc">The parsed as UTC.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime Prepare(this DateTime dateTime, bool parsedAsUtc = false)
        {
            var config = JsConfig.GetConfig();
            if (config.SkipDateTimeConversion)
                return dateTime;

            if (config.AlwaysUseUtc)
                return dateTime.Kind != DateTimeKind.Utc ? dateTime.ToStableUniversalTime() : dateTime;

            return parsedAsUtc ? dateTime.ToLocalTime() : dateTime;
        }

        /// <summary>
        /// Parses the shortest nullable XSD date time.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.DateTime?.</returns>
        public static DateTime? ParseShortestNullableXsdDateTime(string dateTimeStr)
        {
            if (string.IsNullOrEmpty(dateTimeStr))
                return null;

            return ParseShortestXsdDateTime(dateTimeStr);
        }

        /// <summary>
        /// Parses the rf C1123 date time.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime ParseRFC1123DateTime(string dateTimeStr)
        {
            return DateTime.ParseExact(dateTimeStr, "r", CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses the shortest XSD date time.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime ParseShortestXsdDateTime(string dateTimeStr)
        {
            try
            {
                if (string.IsNullOrEmpty(dateTimeStr))
                    return DateTime.MinValue;

                if (dateTimeStr.StartsWith(EscapedWcfJsonPrefix, StringComparison.Ordinal) || dateTimeStr.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
                    return ParseWcfJsonDate(dateTimeStr).Prepare();

                var config = JsConfig.GetConfig();
                if (dateTimeStr.Length == DefaultDateTimeFormat.Length)
                {
                    var unspecifiedDate = DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture);

                    if (config.AssumeUtc)
                        unspecifiedDate = DateTime.SpecifyKind(unspecifiedDate, DateTimeKind.Utc);

                    return unspecifiedDate.Prepare();
                }

                var hasUtcSuffix = dateTimeStr.EndsWith(XsdUtcSuffix);
                if (!hasUtcSuffix && dateTimeStr.Length == DefaultDateTimeFormatWithFraction.Length)
                {
                    var unspecifiedDate = config.AssumeUtc
                        ? DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture, DateTimeStyles.AssumeUniversal)
                        : DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture);

                    return unspecifiedDate.Prepare();
                }

                var kind = hasUtcSuffix
                    ? DateTimeKind.Utc
                    : DateTimeKind.Unspecified;
                switch (config.DateHandler)
                {
                    case DateHandler.UnixTime:
                        if (int.TryParse(dateTimeStr, out var unixTime))
                            return unixTime.FromUnixTime();
                        break;
                    case DateHandler.UnixTimeMs:
                        if (long.TryParse(dateTimeStr, out var unixTimeMs))
                            return unixTimeMs.FromUnixTimeMs();
                        break;
                    case DateHandler.ISO8601:
                    case DateHandler.ISO8601DateOnly:
                    case DateHandler.ISO8601DateTime:
                        if (config.SkipDateTimeConversion)
                            dateTimeStr = RemoveUtcOffsets(dateTimeStr, out kind);
                        break;
                }

                dateTimeStr = RepairXsdTimeSeparator(dateTimeStr);

                if (dateTimeStr.Length == XsdDateTimeFormatSeconds.Length)
                    return DateTime.ParseExact(dateTimeStr, XsdDateTimeFormatSeconds, null, DateTimeStyles.AdjustToUniversal).Prepare(true);

                if (dateTimeStr.Length >= XsdDateTimeFormat3F.Length
                    && dateTimeStr.Length <= XsdDateTimeFormat.Length
                    && hasUtcSuffix)
                {
                    var dateTime = Env.IsMono ? ParseManual(dateTimeStr) : null;
                    return dateTime ?? PclExport.Instance.ParseXsdDateTimeAsUtc(dateTimeStr);
                }

                if (dateTimeStr.Length == CondensedDateTimeFormat.Length && dateTimeStr.IndexOfAny(DateTimeSeparators) == -1)
                {
                    return DateTime.ParseExact(dateTimeStr, "yyyyMMdd", CultureInfo.InvariantCulture, DateTimeStyles.None);
                }

                if (dateTimeStr.Length == ShortDateTimeFormat.Length)
                {
                    try
                    {
                        var manualDate = ParseManual(dateTimeStr);
                        if (manualDate != null)
                            return manualDate.Value;
                    }
                    catch
                    {
                        // ignored
                    }
                }

                try
                {
                    if (config.SkipDateTimeConversion)
                    {
                        var dateTimeStyle = kind == DateTimeKind.Unspecified
                            ? DateTimeStyles.None
                            : kind == DateTimeKind.Local
                                ? DateTimeStyles.AssumeLocal
                                : DateTimeStyles.AssumeUniversal;
                        if (config.AlwaysUseUtc)
                            dateTimeStyle |= DateTimeStyles.AdjustToUniversal;
                        return DateTime.Parse(dateTimeStr, null, dateTimeStyle);
                    }

                    var assumeKind = config.AssumeUtc ? DateTimeStyles.AssumeUniversal : DateTimeStyles.AssumeLocal;
                    var dateTime = DateTime.Parse(dateTimeStr, CultureInfo.InvariantCulture, assumeKind);
                    return dateTime.Prepare();
                }
                catch (FormatException)
                {
                    var manualDate = ParseManual(dateTimeStr);
                    if (manualDate != null)
                        return manualDate.Value;

                    throw;
                }
            }
            catch (Exception ex)
            {
                if (OnParseErrorFn != null)
                    return OnParseErrorFn(dateTimeStr, ex);

                throw;
            }
        }

        /// <summary>
        /// Removes the UTC offsets.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <param name="kind">The kind.</param>
        /// <returns>string.</returns>
        private static string RemoveUtcOffsets(string dateTimeStr, out DateTimeKind kind)
        {
            var startOfTz = UtcOffsetInfoRegex.Match(dateTimeStr);
            if (startOfTz.Index > 0)
            {
                kind = DateTimeKind.Local;
                return dateTimeStr.Substring(0, startOfTz.Index);
            }
            kind = dateTimeStr.Contains("Z") ? DateTimeKind.Utc : DateTimeKind.Unspecified;
            return dateTimeStr;
        }

        /// <summary>
        /// Repairs an out-of-spec XML date/time string which incorrectly uses a space instead of a 'T' to separate the date from the time.
        /// These string are occasionally generated by SQLite and can cause errors in OrmLite when reading these columns from the DB.
        /// </summary>
        /// <param name="dateTimeStr">The XML date/time string to repair</param>
        /// <returns>The repaired string. If no repairs were made, the original string is returned.</returns>
        private static string RepairXsdTimeSeparator(string dateTimeStr)
        {
            if (dateTimeStr.Length > XsdTimeSeparatorIndex && dateTimeStr[XsdTimeSeparatorIndex] == ' ' && dateTimeStr.EndsWith(XsdUtcSuffix))
            {
                dateTimeStr = dateTimeStr.Substring(0, XsdTimeSeparatorIndex) + XsdTimeSeparator +
                              dateTimeStr.Substring(XsdTimeSeparatorIndex + 1);
            }

            return dateTimeStr;
        }

        /// <summary>
        /// Parses the manual.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.DateTime?.</returns>
        public static DateTime? ParseManual(string dateTimeStr)
        {
            var config = JsConfig.GetConfig();
            var dateKind = config.AssumeUtc || config.AlwaysUseUtc
                ? DateTimeKind.Utc
                : DateTimeKind.Local;

            var date = ParseManual(dateTimeStr, dateKind);
            if (date == null)
                return null;

            return dateKind == DateTimeKind.Local
                ? date.Value.ToLocalTime().Prepare()
                : date;
        }

        /// <summary>
        /// Parses the manual.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <param name="dateKind">Kind of the date.</param>
        /// <returns>System.DateTime?.</returns>
        public static DateTime? ParseManual(string dateTimeStr, DateTimeKind dateKind)
        {
            if (dateTimeStr == null || dateTimeStr.Length < ShortDateTimeFormat.Length)
                return null;

            if (dateTimeStr.EndsWith(XsdUtcSuffix))
            {
                dateTimeStr = dateTimeStr.Substring(0, dateTimeStr.Length - 1);
                dateKind = JsConfig.SkipDateTimeConversion ? DateTimeKind.Utc : dateKind;
            }

            var parts = dateTimeStr.Split('T');
            if (parts.Length == 1)
                parts = dateTimeStr.SplitOnFirst(' ');

            var dateParts = parts[0].Split('-', '/');
            int hh = 0, min = 0, ss = 0, ms = 0;
            double subMs = 0;
            int offsetMultiplier = 0;

            switch (parts.Length)
            {
                case 1:
                    return dateParts.Length == 3 && dateParts[2].Length == "YYYY".Length
                        ? new DateTime(int.Parse(dateParts[2]), int.Parse(dateParts[1]), int.Parse(dateParts[0]), 0, 0, 0, 0,
                            dateKind)
                        : new DateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), 0, 0, 0, 0,
                            dateKind);
                case 2:
                {
                    var timeStringParts = parts[1].Split('+');
                    if (timeStringParts.Length == 2)
                    {
                        offsetMultiplier = -1;
                    }
                    else
                    {
                        timeStringParts = parts[1].Split('-');
                        if (timeStringParts.Length == 2)
                        {
                            offsetMultiplier = 1;
                        }
                    }

                    var timeOffset = timeStringParts.Length == 2 ? timeStringParts[1] : null;
                    var timeParts = timeStringParts[0].Split(':');

                    if (timeParts.Length == 3)
                    {
                        int.TryParse(timeParts[0], out hh);
                        int.TryParse(timeParts[1], out min);

                        var secParts = timeParts[2].Split('.');
                        int.TryParse(secParts[0], out ss);
                        if (secParts.Length == 2)
                        {
                            var msStr = secParts[1].PadRight(3, '0');
                            ms = int.Parse(msStr.Substring(0, 3));

                            if (msStr.Length > 3)
                            {
                                var subMsStr = msStr.Substring(3);
                                subMs = double.Parse(subMsStr) / Math.Pow(10, subMsStr.Length);
                            }
                        }
                    }

                    var dateTime = new DateTime(int.Parse(dateParts[0]), int.Parse(dateParts[1]), int.Parse(dateParts[2]), hh, min,
                        ss, ms, dateKind);
                    if (subMs != 0)
                    {
                        dateTime = dateTime.AddMilliseconds(subMs);
                    }

                    if (offsetMultiplier != 0 && timeOffset != null)
                    {
                        timeParts = timeOffset.Split(':');
                        if (timeParts.Length == 2)
                        {
                            hh = int.Parse(timeParts[0]);
                            min = int.Parse(timeParts[1]);
                        }
                        else
                        {
                            hh = int.Parse(timeOffset.Substring(0, 2));
                            min = int.Parse(timeOffset.Substring(2));
                        }

                        dateTime = dateTime.AddHours(offsetMultiplier * hh);
                        dateTime = dateTime.AddMinutes(offsetMultiplier * min);
                    }

                    return dateTime;
                }
                default:
                    return null;
            }
        }

        /// <summary>
        /// Converts to datetimestring.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>string.</returns>
        public static string ToDateTimeString(DateTime dateTime)
        {
            return dateTime.ToStableUniversalTime().ToString(XsdDateTimeFormat, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses the date time.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime ParseDateTime(string dateTimeStr)
        {
            return DateTime.ParseExact(dateTimeStr, XsdDateTimeFormat, null);
        }

        /// <summary>
        /// Parses the date time offset.
        /// </summary>
        /// <param name="dateTimeOffsetStr">The date time offset string.</param>
        /// <returns>System.DateTimeOffset.</returns>
        public static DateTimeOffset ParseDateTimeOffset(string dateTimeOffsetStr)
        {
            if (string.IsNullOrEmpty(dateTimeOffsetStr)) return default(DateTimeOffset);

            // for interop, do not assume format based on config
            // format: prefer TimestampOffset, DCJSCompatible
            if (dateTimeOffsetStr.StartsWith(EscapedWcfJsonPrefix, StringComparison.Ordinal) ||
                dateTimeOffsetStr.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
            {
                return ParseWcfJsonDateOffset(dateTimeOffsetStr);
            }

            // format: next preference ISO8601
            // assume utc when no offset specified
            if (dateTimeOffsetStr.LastIndexOfAny(TimeZoneChars) < 10)
            {
                if (!dateTimeOffsetStr.EndsWith(XsdUtcSuffix)) dateTimeOffsetStr += XsdUtcSuffix;
                if (Env.IsMono)
                {
                    // Without that Mono uses a Local timezone))
                    dateTimeOffsetStr = dateTimeOffsetStr.Substring(0, dateTimeOffsetStr.Length - 1) + "+00:00";
                }
            }

            return DateTimeOffset.Parse(dateTimeOffsetStr, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses the nullable date time offset.
        /// </summary>
        /// <param name="dateTimeOffsetStr">The date time offset string.</param>
        /// <returns>System.DateTimeOffset?.</returns>
        public static DateTimeOffset? ParseNullableDateTimeOffset(string dateTimeOffsetStr)
        {
            if (string.IsNullOrEmpty(dateTimeOffsetStr)) return null;

            return ParseDateTimeOffset(dateTimeOffsetStr);
        }

        /// <summary>
        /// Converts to localxsddatetimestring.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>string.</returns>
        public static string ToLocalXsdDateTimeString(DateTime dateTime)
        {
            return PclExport.Instance.ToLocalXsdDateTimeString(dateTime);
        }

        /// <summary>
        /// Converts to xsdtimespanstring.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>string.</returns>
        public static string ToXsdTimeSpanString(TimeSpan timeSpan)
        {
            return TimeSpanConverter.ToXsdDuration(timeSpan);
        }

        /// <summary>
        /// Converts to xsdtimespanstring.
        /// </summary>
        /// <param name="timeSpan">The time span.</param>
        /// <returns>string.</returns>
        public static string ToXsdTimeSpanString(TimeSpan? timeSpan)
        {
            return timeSpan != null ? ToXsdTimeSpanString(timeSpan.Value) : null;
        }

        /// <summary>
        /// Parses the time span.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.TimeSpan.</returns>
        public static TimeSpan ParseTimeSpan(string dateTimeStr)
        {
            return dateTimeStr.StartsWith("P", StringComparison.Ordinal) || dateTimeStr.StartsWith("-P", StringComparison.Ordinal)
                ? ParseXsdTimeSpan(dateTimeStr)
                : dateTimeStr.Contains(":")
                ? TimeSpan.Parse(dateTimeStr)
                : ParseNSTimeInterval(dateTimeStr);
        }

        /// <summary>
        /// Parses the ns time interval.
        /// </summary>
        /// <param name="doubleInSecs">The double in secs.</param>
        /// <returns>System.TimeSpan.</returns>
        public static TimeSpan ParseNSTimeInterval(string doubleInSecs)
        {
            var secs = double.Parse(doubleInSecs, CultureInfo.InvariantCulture);
            return TimeSpan.FromSeconds(secs);
        }

        /// <summary>
        /// Parses the nullable time span.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.TimeSpan?.</returns>
        public static TimeSpan? ParseNullableTimeSpan(string dateTimeStr)
        {
            return string.IsNullOrEmpty(dateTimeStr)
                ? (TimeSpan?)null
                : ParseTimeSpan(dateTimeStr);
        }

        /// <summary>
        /// Parses the XSD time span.
        /// </summary>
        /// <param name="dateTimeStr">The date time string.</param>
        /// <returns>System.TimeSpan.</returns>
        public static TimeSpan ParseXsdTimeSpan(string dateTimeStr)
        {
            return TimeSpanConverter.FromXsdDuration(dateTimeStr);
        }

        /// <summary>
        /// Converts to shortestxsddatetimestring.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>string.</returns>
        public static string ToShortestXsdDateTimeString(DateTime dateTime)
        {
            var config = JsConfig.GetConfig();

            dateTime = dateTime.UseConfigSpecifiedSetting();
            if (!string.IsNullOrEmpty(config.DateTimeFormat))
            {
                return dateTime.ToString(config.DateTimeFormat, CultureInfo.InvariantCulture);
            }

            var timeOfDay = dateTime.TimeOfDay;
            var isStartOfDay = timeOfDay.Ticks == 0;
            if (isStartOfDay && !config.SkipDateTimeConversion)
                return dateTime.ToString(ShortDateTimeFormat, CultureInfo.InvariantCulture);

            var hasFractionalSecs = timeOfDay.Milliseconds != 0
                || timeOfDay.Ticks % TimeSpan.TicksPerMillisecond != 0;

            if (config.SkipDateTimeConversion)
            {
                if (!hasFractionalSecs)
                    return dateTime.Kind == DateTimeKind.Local
                        ? dateTime.ToString(DateTimeFormatSecondsUtcOffset, CultureInfo.InvariantCulture)
                        : dateTime.Kind == DateTimeKind.Unspecified
                        ? dateTime.ToString(DateTimeFormatSecondsNoOffset, CultureInfo.InvariantCulture)
                        : dateTime.ToStableUniversalTime().ToString(XsdDateTimeFormatSeconds, CultureInfo.InvariantCulture);

                return dateTime.Kind == DateTimeKind.Local
                    ? dateTime.ToString(DateTimeFormatTicksUtcOffset, CultureInfo.InvariantCulture)
                    : dateTime.Kind == DateTimeKind.Unspecified
                    ? dateTime.ToString(DateTimeFormatTicksNoUtcOffset, CultureInfo.InvariantCulture)
                    : PclExport.Instance.ToXsdDateTimeString(dateTime);
            }

            if (!hasFractionalSecs)
                return dateTime.Kind != DateTimeKind.Utc
                    ? dateTime.ToString(DateTimeFormatSecondsUtcOffset, CultureInfo.InvariantCulture)
                    : dateTime.ToStableUniversalTime().ToString(XsdDateTimeFormatSeconds, CultureInfo.InvariantCulture);

            return dateTime.Kind != DateTimeKind.Utc
                ? dateTime.ToString(DateTimeFormatTicksUtcOffset, CultureInfo.InvariantCulture)
                : PclExport.Instance.ToXsdDateTimeString(dateTime);
        }

        /// <summary>
        /// The time zone chars
        /// </summary>
        static readonly char[] TimeZoneChars = new[] { '+', '-' };

        /// <summary>
        /// The minimum date time offset WCF value
        /// </summary>
        private const string MinDateTimeOffsetWcfValue = "\\/Date(-62135596800000)\\/";
        /// <summary>
        /// The maximum date time offset WCF value
        /// </summary>
        private const string MaxDateTimeOffsetWcfValue = "\\/Date(253402300799999)\\/";

        /// <summary>
        /// WCF Json format: /Date(unixts+0000)/
        /// </summary>
        /// <param name="wcfJsonDate">The WCF json date.</param>
        /// <returns>System.DateTimeOffset.</returns>
        public static DateTimeOffset ParseWcfJsonDateOffset(string wcfJsonDate)
        {
            switch (wcfJsonDate)
            {
                case MinDateTimeOffsetWcfValue:
                    return DateTimeOffset.MinValue;
                case MaxDateTimeOffsetWcfValue:
                    return DateTimeOffset.MaxValue;
            }

            if (wcfJsonDate[0] == '\\')
            {
                wcfJsonDate = wcfJsonDate.Substring(1);
            }

            var suffixPos = wcfJsonDate.IndexOf(WcfJsonSuffix);
            var timeString = suffixPos < 0 ? wcfJsonDate : wcfJsonDate.Substring(WcfJsonPrefix.Length, suffixPos - WcfJsonPrefix.Length);

            // for interop, do not assume format based on config
            if (!wcfJsonDate.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
            {
                return DateTimeOffset.Parse(timeString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }

            var timeZonePos = timeString.LastIndexOfAny(TimeZoneChars);
            var timeZone = timeZonePos <= 0 ? string.Empty : timeString.Substring(timeZonePos);
            var unixTimeString = timeString.Substring(0, timeString.Length - timeZone.Length);

            var unixTime = long.Parse(unixTimeString);

            if (timeZone == string.Empty)
            {
                // when no timezone offset is supplied, then treat the time as UTC
                return unixTime.FromUnixTimeMs();
            }

            // DCJS ignores the offset and considers it local time if any offset exists
            // REVIEW: DCJS shoves offset in a separate field 'offsetMinutes', we have the offset in the format, so shouldn't we use it?
            if (JsConfig.DateHandler == DateHandler.DCJSCompatible || timeZone == UnspecifiedOffset)
            {
                return unixTime.FromUnixTimeMs().ToLocalTime();
            }

            var offset = timeZone.FromTimeOffsetString();
            var date = unixTime.FromUnixTimeMs();
            return new DateTimeOffset(date.Ticks, offset);
        }

        /// <summary>
        /// WCF Json format: /Date(unixts+0000)/
        /// </summary>
        /// <param name="wcfJsonDate">The WCF json date.</param>
        /// <returns>System.DateTime.</returns>
        public static DateTime ParseWcfJsonDate(string wcfJsonDate)
        {
            if (wcfJsonDate[0] == JsonUtils.EscapeChar)
            {
                wcfJsonDate = wcfJsonDate.Substring(1);
            }

            var suffixPos = wcfJsonDate.IndexOf(WcfJsonSuffix);
            var timeString = wcfJsonDate.Substring(WcfJsonPrefix.Length, suffixPos - WcfJsonPrefix.Length);

            // for interop, do not assume format based on config
            if (!wcfJsonDate.StartsWith(WcfJsonPrefix, StringComparison.Ordinal))
            {
                return DateTime.Parse(timeString, CultureInfo.InvariantCulture, DateTimeStyles.RoundtripKind);
            }

            var timeZonePos = timeString.LastIndexOfAny(TimeZoneChars);
            var timeZone = timeZonePos <= 0 ? string.Empty : timeString.Substring(timeZonePos);
            var unixTimeString = timeString.Substring(0, timeString.Length - timeZone.Length);

            var unixTime = long.Parse(unixTimeString);

            if (timeZone == string.Empty)
            {
                // when no timezone offset is supplied, then treat the time as UTC
                return unixTime.FromUnixTimeMs();
            }

            // DCJS ignores the offset and considers it local time if any offset exists
            if (JsConfig.DateHandler == DateHandler.DCJSCompatible || timeZone == UnspecifiedOffset)
            {
                return unixTime.FromUnixTimeMs().ToLocalTime();
            }

            var offset = timeZone.FromTimeOffsetString();
            var date = unixTime.FromUnixTimeMs(offset);
            return date;
        }

        /// <summary>
        /// Gets the local time zone information.
        /// </summary>
        /// <returns>System.TimeZoneInfo.</returns>
        public static TimeZoneInfo GetLocalTimeZoneInfo()
        {
            try
            {
                return TimeZoneInfo.Local;
            }
            catch (Exception)
            {
                return TimeZoneInfo.Utc; //Fallback for Mono on Windows.
            }
        }

        /// <summary>
        /// The local time zone
        /// </summary>
        internal static TimeZoneInfo LocalTimeZone = GetLocalTimeZoneInfo();

        /// <summary>
        /// Uses the configuration specified setting.
        /// </summary>
        /// <param name="dateTime">The date time.</param>
        /// <returns>System.DateTime.</returns>
        private static DateTime UseConfigSpecifiedSetting(this DateTime dateTime)
        {
            if (JsConfig.AssumeUtc && dateTime.Kind == DateTimeKind.Unspecified)
            {
                return DateTime.SpecifyKind(dateTime, DateTimeKind.Utc);
            }
            return dateTime;
        }

        /// <summary>
        /// Writes the WCF json date.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="dateTime">The date time.</param>
        public static void WriteWcfJsonDate(TextWriter writer, DateTime dateTime)
        {
            var config = JsConfig.GetConfig();

            dateTime = dateTime.UseConfigSpecifiedSetting();
            switch (config.DateHandler)
            {
                case DateHandler.ISO8601:
                    writer.Write(dateTime.ToString("o", CultureInfo.InvariantCulture));
                    return;
                case DateHandler.ISO8601DateOnly:
                    writer.Write(dateTime.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
                    return;
                case DateHandler.ISO8601DateTime:
                    writer.Write(dateTime.ToString("yyyy-MM-dd HH:mm:ss", CultureInfo.InvariantCulture));
                    return;
                case DateHandler.RFC1123:
                    writer.Write(dateTime.ToUniversalTime().ToString("R", CultureInfo.InvariantCulture));
                    return;
            }

            var timestamp = dateTime.ToUnixTimeMs();
            string offset = null;
            if (dateTime.Kind != DateTimeKind.Utc)
            {
                if (config.DateHandler == DateHandler.TimestampOffset && dateTime.Kind == DateTimeKind.Unspecified)
                    offset = UnspecifiedOffset;
                else
                    offset = LocalTimeZone.GetUtcOffset(dateTime).ToTimeOffsetString();
            }
            else
            {
                // Normally the JsonDateHandler.TimestampOffset doesn't append an offset for Utc dates, but if
                // the config.AppendUtcOffset is set then we will
                if (config.DateHandler == DateHandler.TimestampOffset && config.AppendUtcOffset)
                    offset = UtcOffset;
            }

            writer.Write(EscapedWcfJsonPrefix);
            writer.Write(timestamp);
            if (offset != null)
            {
                writer.Write(offset);
            }
            writer.Write(EscapedWcfJsonSuffix);
        }

        /// <summary>
        /// Writes the WCF json date time offset.
        /// </summary>
        /// <param name="writer">The writer.</param>
        /// <param name="dateTimeOffset">The date time offset.</param>
        public static void WriteWcfJsonDateTimeOffset(TextWriter writer, DateTimeOffset dateTimeOffset)
        {
            if (JsConfig.DateHandler == DateHandler.ISO8601)
            {
                writer.Write(dateTimeOffset.ToString("o", CultureInfo.InvariantCulture));
                return;
            }

            var timestamp = dateTimeOffset.Ticks.ToUnixTimeMs();
            var offset = dateTimeOffset.Offset == TimeSpan.Zero
                ? null
                : dateTimeOffset.Offset.ToTimeOffsetString();

            writer.Write(EscapedWcfJsonPrefix);
            writer.Write(timestamp);
            if (offset != null)
            {
                writer.Write(offset);
            }
            writer.Write(EscapedWcfJsonSuffix);
        }
    }
}