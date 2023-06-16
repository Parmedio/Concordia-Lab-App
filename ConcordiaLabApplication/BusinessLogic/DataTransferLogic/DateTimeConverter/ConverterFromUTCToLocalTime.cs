namespace BusinessLogic.DataTransferLogic.DateTimeConverter;

internal static class ConverterFromUTCToLocalTime
{
    private static readonly TimeSpan offSet = new TimeSpan(11, 0, 0);
    private static readonly string concordiaStationId = "Concordia Station";
    private static readonly string concordiaStationTimeZoneDisplayName = "Concordia Station";

    private static readonly TimeZoneInfo antartideTimeZone = TimeZoneInfo.CreateCustomTimeZone(concordiaStationId, offSet, concordiaStationTimeZoneDisplayName, concordiaStationId);

    internal static DateTime? ConvertToAntartideTimeZone(this DateTime? dateTimeToConvert)
        => dateTimeToConvert is null ? null : TimeZoneInfo.ConvertTimeFromUtc(dateTimeToConvert ?? DateTime.UtcNow, antartideTimeZone);


}
