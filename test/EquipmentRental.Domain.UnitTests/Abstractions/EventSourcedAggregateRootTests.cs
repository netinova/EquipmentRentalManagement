using EquipmentRental.Domain.Abstractions;
using FluentAssertions;

namespace EquipmentRental.Domain.UnitTests.Abstractions;

public class EventSourcedAggregateRootTests
{
    [Fact]
    public void RaiseAndApplyEvent_ShouldApplyEvent_WhenCalled()
    {
        // Arrange
        var aggregate = new TestAggregate();
        var @event = new TestDomainEvent();

        // Act
        aggregate.TestRaiseAndApplyEvent(@event);

        // Assert
        aggregate.LastAppliedEvent.Should().Be(@event);
    }

    [Fact]
    public void RaiseAndApplyEvent_ShouldRaiseEvent_WhenCalled()
    {
        // Arrange
        var aggregate = new TestAggregate();
        var @event = new TestDomainEvent();

        // Act
        aggregate.TestRaiseAndApplyEvent(@event);

        // Assert
        aggregate.DomainEvents.Should().ContainSingle();
        aggregate.DomainEvents.First().Should().Be(@event);
    }

    [Fact]
    public void RaiseAndApplyEvent_ShouldApplyAndRaiseSameEvent_WhenCalled()
    {
        // Arrange
        var aggregate = new TestAggregate();
        var @event = new TestDomainEvent();

        // Act
        aggregate.TestRaiseAndApplyEvent(@event);

        // Assert
        aggregate.LastAppliedEvent.Should().BeSameAs(@event);
        aggregate.DomainEvents.Last().Should().BeSameAs(@event);
    }

    [Fact]
    public void RaiseAndApplyEvent_ShouldStoreMultipleEvents_WhenCalledMultipleTimes()
    {
        // Arrange
        var aggregate = new TestAggregate();
        var event1 = new TestDomainEvent();
        var event2 = new TestDomainEvent();

        // Act
        aggregate.TestRaiseAndApplyEvent(event1);
        aggregate.TestRaiseAndApplyEvent(event2);

        // Assert
        aggregate.DomainEvents.Should().HaveCount(2);
        aggregate.DomainEvents.ElementAt(0).Should().Be(event1);
        aggregate.DomainEvents.ElementAt(1).Should().Be(event2);
    }

    private class TestAggregate : EventSourcedAggregateRoot
    {
        public IDomainEvent? LastAppliedEvent { get; private set; }

        public void TestRaiseAndApplyEvent(IDomainEvent domainEvent)
            => RaiseAndApplyEvent(domainEvent);

        protected override void Apply(IDomainEvent domainEvent)
            => LastAppliedEvent = domainEvent;
    }

    private class TestDomainEvent : IDomainEvent
    {
        public DateTime OccurredOn => DateTime.UtcNow;
        public Guid StreamId => Guid.NewGuid();
    }
}
