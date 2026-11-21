# Framewave Rentals --- Project Specification

**Status:** Baseline\
**Version:** 1.1\
**Updated:** 21 November 2026\
**Project:** Portfolio backend / modular monolith\
**Domain:** Professional photography and audiovisual equipment rental

## 1. Product scope

Framewave Rentals is a single rental company operating multiple branches.
It rents professional photography and audiovisual equipment to
individuals and organizations.

The system supports: - Product catalog and physical inventory
management. - Customer and branch management. - Availability queries and
reservations. - Allocation of specific equipment units. - Rental
handover and returns. - Equipment inspection and maintenance. - Rental
fees, deposits, and additional charges. - Staff access control and
operational history.

The system is not a marketplace and does not support multiple
independent rental companies.

### Out of scope

- Delivery, shipping, and inter-branch transfers.
- Cross-branch returns.
- Real payment gateway integration.
- Accounting, tax compliance, or a general ledger.
- Discounts, loyalty programs, and dynamic pricing.
- Advanced insurance or damage-claim workflows.
- Microservices, event sourcing, Kafka, and Kubernetes.

## 2. Domain concepts

---

Concept Responsibility

---

**Product** Catalog definition customers can
request, such as a camera model or
lens.

**Equipment Unit** Individually tracked physical item
belonging to a Product.

**Branch** Company location where equipment is
stored, collected, returned, or
maintained.

**Customer** Person or organization renting
equipment.

**Reservation** Request and commitment for product
quantities, a rental period, and a
pickup branch.

**Reservation Line** Product and quantity requested in a
Reservation.

**Allocation** Commitment of a specific Equipment
Unit to a Reservation for a period.

**Rental** Record created at handover,
identifying the units actually
given to the customer.

**Return** Receipt and inspection of one or
more rented units.

**Maintenance Record** Record of inspection, repair, or
servicing that can block a unit
from rental.

**Charge** Amount owed in connection with a
Rental, such as rental fees, late
fees, or damage charges.

**Deposit** Refundable security amount
associated with a Rental.

---

A Product is not a physical item. A Product may have multiple Equipment
Units. Reservations request Products and quantities; Allocations and
Rentals identify physical Equipment Units.

## 3. Functional requirements

### 3.1 Product catalog

A Product must contain: - Identifier. - Name and description. -
Category. - Manufacturer and model information. - Rental rate and
pricing basis. - Rental eligibility (whether the Product is currently
offered for rental). - Optional handling notes.

A Product may have zero or more Equipment Units. Disabling a Product
prevents new reservations but preserves its history.

### 3.2 Equipment inventory

Each Equipment Unit must contain: - Identifier. - Product reference. -
Serial number and/or company asset tag. - Current Branch. - Operational
status. - Recorded condition. - Registration or acquisition information
where applicable.

Operational status: - **InService:** operationally eligible for
rental. - **UnderMaintenance:** unavailable while maintenance or
inspection is outstanding. - **Retired:** permanently removed from
rental circulation.

Operational status is distinct from reservation and rental state. A unit
may be committed to a future Reservation while remaining in service.
Availability is determined using operational eligibility, branch,
requested period, allocations, and active rentals.

Equipment Units with historical records must be retired rather than
deleted. Important status changes must be retained in history.

### 3.3 Branches

A Branch must contain: - Identifier. - Name. - Address and contact
information. - Active/inactive status.

A Reservation specifies a pickup Branch. In the first version, the same
Branch is used for return. Cross-branch returns and transfers are
excluded.

Deactivating a Branch must preserve its historical records.

### 3.4 Customers

A Customer profile must contain: - Identifier. - Name. - Contact
information. - Account status. - Registration timestamp. - Verification
or rental-eligibility status.

Customer status: - **Active:** may create reservations, subject to
eligibility rules. - **Suspended:** may not create reservations or
collect new rentals. - **Closed:** inactive; historical records remain
available according to access and retention rules.

The first version uses a staff-managed verification/eligibility status.
Identity-document verification and credit scoring are excluded.

### 3.5 Reservations

A Reservation must contain: - Identifier. - Customer. - Pickup Branch. -
Rental period. - One or more Reservation Lines. - Status. - Confirmed
price quote.

Each Reservation Line contains a Product and a quantity greater than
zero.

Reservation lifecycle:

```text
Pending ──> Confirmed ──> Fulfilled
   │             │
   └─────────────┴──────> Cancelled
```

- **Pending:** created but not confirmed.
- **Confirmed:** all requested quantities have been allocated.
- **Fulfilled:** equipment has been handed over and a Rental has been
  created.
- **Cancelled:** reservation will not proceed.

A confirmed Reservation must have a complete inventory commitment.
Confirmation is all-or-nothing: if any line cannot be fulfilled, no
allocations are committed and the Reservation remains unconfirmed.

A confirmed price quote is preserved. Later catalog price changes must
not alter existing confirmed prices.

Reservation expiration is not required in the first version.

### 3.6 Availability and allocation

Availability is evaluated for a Product, quantity, Branch, and requested
period.

Eligible units must: 1. Belong to the requested Product. 2. Be located
at the requested Branch. 3. Be operationally eligible. 4. Have no
conflicting Allocation or active Rental during the requested period. 5.
Be available in sufficient quantity.

At confirmation, the system selects specific Equipment Units and commits
all Allocations atomically.

An Allocation records: - Identifier. - Equipment Unit. - Reservation. -
Committed period. - Allocation state or release information, as required
by the implementation.

Cancelling a confirmed Reservation releases its future Allocations.
Fulfillment links or transfers the commitment to the resulting Rental
without an uncommitted gap.

**Inventory invariant:** the same Equipment Unit must not be committed
to overlapping active Reservations or Rentals. This invariant must hold
under concurrent requests.

An Allocation represents a planned commitment. An active Rental
represents actual possession. A unit remains unavailable while it is
still rented, even if its planned Allocation period has ended. A late
return may therefore affect a later Reservation; the operational process
for resolving that conflict remains an open policy decision.

### 3.7 Rental and handover

Authorized staff perform handover for a confirmed Reservation.

Handover must: 1. Verify the Customer and Reservation. 2. Verify the
Reservation can be fulfilled. 3. Confirm the allocated units being
handed over. 4. Record each unit's condition and included accessories. 5. Record the actual handover timestamp. 6. Create the Rental and
fulfill the Reservation.

A Rental must record: - Identifier. - Customer and source Reservation. -
Pickup/return Branch. - Actual handover timestamp. - Expected return
timestamp. - Equipment Units handed over. - Initial condition and
included accessories. - Agreed rental price and deposit details. -
Rental status.

Rental lifecycle:

```text
Active ──> Returned
```

A Rental remains Active until every unit has been returned and return
processing is complete. Partial returns are supported.

A Reservation must not be fulfilled without its corresponding Rental
being created.

### 3.8 Returns and inspection

Returned units are processed individually. For each unit, record: -
Actual return timestamp. - Observed condition. - Accessory
completeness. - Inspection notes. - Availability or maintenance
outcome. - Any proposed additional charge and its reason.

Return outcomes: - **Good:** may become eligible for rental after return
processing. - **Damaged:** blocked from rental until assessed and
cleared. - **Missing:** remains outstanding and unavailable.

Receiving a unit and completing the Rental return are separate
operations. The Rental remains Active while any unit is outstanding.

A damaged or unsafe unit must remain unavailable until an authorized
assessment or required maintenance is complete.

### 3.9 Maintenance

Inventory staff can create and manage Maintenance Records.

A Maintenance Record contains: - Equipment Unit. - Reason/category and
description. - Opened timestamp. - Status. - Assigned staff member, if
applicable. - Completion timestamp and resolution notes.

Maintenance lifecycle:

```text
Open ──> InProgress ──> Completed
  └───────────────────> Cancelled
```

A unit with open or in-progress maintenance cannot be allocated.
Completing maintenance does not automatically make a unit available; its
operational eligibility must be reassessed.

### 3.10 Pricing and financial records

The first version supports: - Base rental fees. - Deposits. -
Late-return fees. - Damage charges. - Missing-accessory charges.

Rules: - Confirmed prices are preserved on the Reservation/Rental. -
Late fees use the expected and actual return timestamps and a
configurable simple rate. - Additional charges require a staff-entered
reason and amount. - Financial outcomes and deposit status are
recorded. - Financial corrections must be represented by an adjustment,
reversal, or explicit status change; assessed charges must not be
silently overwritten or deleted.

No real payment gateway is required. Payments may be recorded as
manually collected, simulated, refunded, or outstanding.

### 3.11 Access control

The system has Customer, Rental Clerk, Inventory Manager, and
Administrator roles.

- Customers can access only their own profiles, reservations, rentals,
  and financial records.
- Rental Clerks manage customer-facing reservation, handover, and
  return operations.
- Inventory Managers manage equipment, status, inspections, and
  maintenance.
- Administrators manage staff access, branches, catalog configuration,
  and system settings.
- Staff permissions may be restricted by Branch.
- The acting identity must come from the authenticated principal, not
  from an untrusted client-supplied identifier.

Authorization is enforced in Application use cases.

### 3.12 Operational history

Preserve the history of: - Reservation creation, confirmation,
cancellation, and fulfillment. - Allocation and release. - Handover and
returns. - Inspection outcomes. - Equipment status changes. -
Maintenance lifecycle changes. - Charges, deposits, and financial
adjustments.

History should identify who performed an action, when it occurred, and
which business record it affected.

## 4. Business rules and invariants

---

ID Requirement

---

BR-01 A Reservation must contain at least
one line; every line quantity must
be greater than zero.

BR-02 A rental period must start before
it ends. Timestamps must follow one
documented timezone policy.

BR-03 Only active, eligible Customers may
create Reservations or collect
Rentals.

BR-04 A Reservation must reference an
active Branch and Products
currently offered for rental.

BR-05 A Reservation can be confirmed only
when every requested quantity can
be allocated for the full period.

BR-06 One Equipment Unit cannot have
overlapping active Allocations or
remain allocated while in an active
Rental.

BR-07 Units under maintenance or retired
units cannot be allocated.

BR-08 Cancelling a confirmed Reservation
releases its future Allocations. A
fulfilled Reservation cannot be
cancelled.

BR-09 A Reservation can be fulfilled only
once through an authorized
handover.

BR-10 A Rental must identify the actual
Equipment Units handed over.

BR-11 A Rental cannot be marked Returned
while any unit remains outstanding
or return processing is incomplete.

BR-12 A damaged or unsafe returned unit
cannot become available until
assessed and cleared.

BR-13 Every Charge must have a valid
amount, currency, reason/category,
and Rental reference. Additional
charges require an auditable
explanation.

BR-14 Confirmed prices and financial
records must not be rewritten by
later catalog changes.

BR-15 State transitions must be valid;
repeated commands must not create
duplicate business effects.

BR-16 Records with business history must
be deactivated or retired rather
than physically deleted.

---

### Concurrency

Two customers may attempt to reserve the final eligible unit at the same
time. At most one confirmation may commit that unit for overlapping
periods.

An availability query is informational; availability must be revalidated
during confirmation. Use database transactions and an appropriate
concurrency strategy. Verify the behavior with PostgreSQL integration
tests.

## 5. Main workflows

### Create and confirm a Reservation

1.  Customer requests availability for products, quantities, period, and
    Branch.
2.  System returns eligible quantities.
3.  Customer submits a Reservation.
4.  System validates Customer, Branch, Products, quantities, period, and
    prices.
5.  System selects eligible Equipment Units.
6.  System commits the Reservation and all Allocations atomically.
7.  Reservation becomes Confirmed.

If inventory is insufficient at confirmation time, the operation fails
without partial Allocations.

### Cancel a Reservation

1.  Customer or authorized staff requests cancellation.
2.  System validates that cancellation is allowed.
3.  Reservation becomes Cancelled.
4.  Future Allocations are released atomically.
5.  Cancellation is recorded.

### Hand over equipment

1.  Staff opens a confirmed Reservation.
2.  System verifies its status and Allocations.
3.  Staff verifies the Customer and units.
4.  Staff records condition and accessories.
5.  System creates the Rental and records handover time.
6.  Reservation becomes Fulfilled.

### Return equipment

1.  Staff receives one or more units from an active Rental.
2.  Staff records return time, condition, accessories, and inspection
    notes.
3.  System calculates applicable late fees and records proposed
    additional charges.
4.  Good units become eligible for availability; damaged units remain
    blocked.
5.  Rental remains Active while units are outstanding.
6.  Rental becomes Returned when all units are accounted for and return
    processing is complete.
7.  Deposit settlement and final financial status are recorded.

### Send equipment to maintenance

1.  Inventory Manager opens a Maintenance Record.
2.  Unit is excluded from allocation.
3.  Staff records work and resolution.
4.  Unit is inspected and its operational status is updated.
5.  Maintenance history is retained.

## 6. Queries

The system must support: - Product catalog and product details. -
Availability by Product, quantity, Branch, and period. - Customer
reservation and rental history. - Reservation details and status. -
Active and overdue Rentals. - Equipment Units by Branch and operational
status. - Equipment and maintenance history. - Rental charges and
deposit status. - Basic inventory summaries.

Read responses may use purpose-built DTOs and do not need to mirror
aggregate structure.

## 7. Technical constraints

- **Architecture:** modular monolith using Clean Architecture
  dependency direction.
- **API:** versioned REST API with OpenAPI documentation.
- **Database:** PostgreSQL.
- **Persistence:** EF Core; Dapper or SQL may be used for specific
  read queries where justified.
- **Testing:** domain unit tests, application tests, PostgreSQL
  integration tests, and API tests.
- **Deployment:** Docker-based local development and CI.
- **CQRS:** use purpose-built read models where useful; separate
  databases are not required.
- **Infrastructure:** business rules must remain independent of
  ASP.NET Core, EF Core, and external infrastructure.

Suggested feature areas: Catalog, Inventory, Customers, Reservations,
Rentals, Maintenance, and Finance. These are organizational suggestions,
not mandatory aggregate or project boundaries.

The allocation concurrency invariant must have an integration test using
competing confirmation attempts against PostgreSQL.

## 8. Delivery phases

### Phase 1 --- Catalog and inventory

- Branches.
- Product catalog.
- Equipment Units.
- Operational status and history.
- Basic Customer accounts.
- Migrations and domain tests.

**Completion:** staff can manage catalog and physical inventory while
preserving historical records.

### Phase 2 --- Reservations and availability

- Availability queries.
- Reservation creation.
- Price quote.
- Confirmation and Allocation.
- Cancellation and Allocation release.
- Concurrency-safe inventory commitment.

**Completion:** competing requests cannot commit the same unit for
overlapping periods.

### Phase 3 --- Rental lifecycle

- Handover.
- Rental creation.
- Unit-level and partial returns.
- Inspection and maintenance blocking.
- Rental completion.

**Completion:** equipment can be tracked from Reservation through
handover and return.

### Phase 4 --- Financial records

- Rental fees and deposits.
- Late fees.
- Damage and accessory charges.
- Manual/simulated payments and deposit settlement.
- Auditable adjustments.

**Completion:** each Rental has a traceable financial outcome without a
real payment gateway.

### Phase 5 --- Backend polish

- Authentication and role/Branch authorization.
- Transactional Outbox and retry-safe publisher.
- Logging and health checks.
- OpenAPI documentation.
- Docker-based development and CI.
- PostgreSQL integration tests.
- README, architecture diagram, and ADRs.

**Completion:** another developer can run, test, and understand the
project from its repository documentation.

## 9. Decisions to resolve during implementation

- Timestamp-based or day-based rental periods and billing.
- Timezone policy and branch opening hours.
- Preparation/inspection buffer between rentals.
- Automatic confirmation or staff approval.
- Late-fee formula, grace period, and cap.
- Deposit calculation.
- Conditions under which a damaged unit can be cleared without
  maintenance.
- Whether accessories are individually tracked or represented as
  quantities.
- Database concurrency mechanism for Allocation.
- Operational response when a late return affects a later Reservation.

Resolve each decision when its workflow is implemented and record the
chosen policy.

## 10. Scope controls

Do not add the following before the core workflow is complete: -
Multiple rental companies or tenant isolation. - Delivery, shipping, or
cross-branch transfers/returns. - Discounts, coupons, loyalty programs,
or dynamic pricing. - Insurance and complex damage claims. - Real
payment gateway integration. - Customer messaging or chat. - Advanced
analytics or a separate reporting database. - Microservices, event
sourcing, Kafka, or Kubernetes.

A feature should be added only when it supports a defined business
workflow or a deliberate project learning objective.

## 11. Distributed-systems objectives

The principal distributed-systems feature is a Transactional Outbox: -
Persist an Outbox message in the same database transaction as the
business change. - Publish pending messages using a background worker. -
Make publishing retry-safe. - Make consumers idempotent. - Do not assume
exactly-once delivery.

Suitable initial events include `ReservationConfirmed` and
`RentalStarted`. Do not publish every method call or introduce a message
broker without a concrete need.

Important commands should prevent duplicate business effects. API
idempotency keys may be introduced for operations such as confirmation
and payment recording.

## 12. Design constraints

This specification defines business requirements and scope, not the
final code structure.

- Do not assume every domain concept is an aggregate root.
- Define aggregate boundaries around consistency requirements.
- Keep aggregates as small as possible while protecting invariants.
- Put use-case orchestration in Application.
- Keep persistence and framework concerns in Infrastructure.
- Introduce patterns only to solve a concrete requirement.
- Update this specification when an agreed business requirement or
  project scope changes.
