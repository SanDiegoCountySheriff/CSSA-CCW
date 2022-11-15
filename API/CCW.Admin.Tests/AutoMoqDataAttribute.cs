using AutoFixture.NUnit3;
using AutoFixture;
using AutoFixture.AutoMoq;

namespace CCW.Admin.Tests;

internal class AutoMoqDataAttribute : AutoDataAttribute
{
    public AutoMoqDataAttribute()
        : base(() => new Fixture().Customize(new AutoMoqCustomization()))
    {
    }
}