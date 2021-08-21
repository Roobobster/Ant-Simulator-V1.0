# Ant-Simulator-V1.0
Ant simulator that has colony of ants that work together to find food in similiar ant pheromone style.

Currently has colony which will create a large amount of ants (size can be specified as well as the rate of spawning them and the duration to spawn them over).

There is also food spawners which will spawn set amounts of food in specified location and radius.

Ants will run around randomly till the find food. When they find food they will move right back to the colony and deposit it. When they are returning to home after getting food they will drop a pheromone trail which other ants will also move towardss and inturn lead them to the food source. They will also move toward food source if they are close enough due to food scent like in real life.

This version is a prototype to experiment with the features that could be used in unity to replicate ant behaviour. But the current solution in this version requires a lot of rework for additional features that I want to add such as other ant colonies that will fight over the foods source. It could be done using this current version but will be a lot harder than just reworking the whole project. In addition, in the next version the trail following will be reworked to not require so much collision detection and instead they will follow the trails more strictly and always go in the correct direction. This project was overall to get experience with Unity's features for this project's final version.
