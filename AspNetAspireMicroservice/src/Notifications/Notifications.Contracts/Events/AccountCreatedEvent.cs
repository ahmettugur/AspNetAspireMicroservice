﻿namespace Notifications.Contracts.Events;

public record AccountCreatedEvent(Guid AccountId, Guid ClientId, string ClientName, string ClientEmail);