# CSharp Design Patterns

## Behavioral patterns

1. Mediator 
2. Chain Of Responsibility I
3. Data Pipeline
4. Strategy
5. Visitor
6. Chain of Responsibility II - Chained Workflows

## Mediator Demo: Soccer Match System

Components:
* Scheduler - game broadcasting system
* Field System - game events generator
* Display System - on-screen game information (for demo purpose console is used as the display)

![](docs/mediator-diagram.png)

Pattern advantages (demonstrated in this demo):
* Reduce coupling between various components
* Ease of introduce more output (display) systems
* Communication between systems is collected in one place and is easier to control

Scenarios (not coverd in this demo):
* Two-way communication:
   * [A] => mediator => [B] => mediator => [A]
* Chained communication:
   * [A] => mediator => [B] => mediator => [C]
* Chained mediators (separating two domains)
   * [A] => mediator1 => mediator2 => [B]