using AutoFixture;
using AutoFixture.AutoMoq;
using AutoFixture.NUnit3;


namespace CCW.Application.Tests;

internal class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}