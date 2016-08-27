#Parrot
**Parrot** is an **I**nternet **o**f **T**hings (**IoT**) simulation platform built on top of Microsoft's Service Fabric.
The platform extends a simple replay engine by allowing user interactions which can modify the assets in the network to change the original flow of data. Each device will send events and recieve commands from an a cloud gateway. This allows your business logic to sit behind the gateway, process the event stream and control the simulation remotely. Parrot simulations also have the first class notion of time and change the tempo of the simulation. 

The following flow depicts the movement of data through the system.

[Generate/Read Data] --> [Schedule] --> [Transform] --> [Execute]

---

##Concepts
### Universe
A universe comrpises of all the services required to run an individual simulation.
###Universe Specification
Contains references to all the relevant files required to build a universe.
###Universe Definition
Captures metadata about a universe and any dedicated service endpoints.
###Universe Templates
A universe template is a file that describes all of the assets in your IoT network.

###Universe Event Stream
A universe event stream is a data file with all the real sensor traffic from the network described in your universe template file.

###Cloud Gateway
A cloud gateway represents a bi-directional cloud service that supports device management such as Azure IoT Hub.
 
---

##Shared Services
These services are shared between all universes.

###UniverseFactory
Orchestrates the creation of a new universe and registers it in a shared registry.

###UniverseTemplateLoader
Responsible for loading the universe template file and creating an in memory representation.

###UniverseBuilder
Reads a universe template and creates all the required actors and dedicated services.

###UniverseManagementWebApi
A common web api to allow clients to manage their simulations.

###UniverseRegistry
A shared registry to store metadata about each running universe.

---

##Dedicated Services
These services are dedicated to each universe.
###UniverseActorService
A service to host all the actors which represent assest in the IoT network.
###UniverseScheduler
Reads sensor events from a source event file and schedules tasks for each actor to run.
###UniverseActorRegistry
A store to map all the external asset ids to internal actor ids for this simulation.
