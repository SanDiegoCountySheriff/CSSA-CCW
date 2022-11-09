namespace CCW.Schedule.Mappers;

public interface IMapper<in TSourceType, out TDestination>
{
    TDestination Map(TSourceType sourceType);
}

public interface IMapper<in TSourceType1, in TSourceType2, out TDestinationType>
{
    TDestinationType Map(TSourceType1 source1, TSourceType2 source2);
}