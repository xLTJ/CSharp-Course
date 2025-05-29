# Main Modules

- **Control Module**: Module that controls all the other modules
- **Navigation/Movement Module**: Responsible for making the robot navigate
- **Garbage Collection Module**: Handles garbage collection
- **User Interface (UI) Module**: Allows the staff to interact with the robot
- **Sensor Module**: Handles the sensors the robots needs to do things, such as detecting garbage and obstacles
- **Schedule Module**: Handles automatic scheduling

# Use cases
All use cases inherently includes the control module.

## Staff use cases

### Start Cleaning
- UI Module
- Navigation/Movement Module (start robot movement)
- Schedule Module (Used to start cleaning based on schedule)

### Stop Robot
- UI Module
- Navigation/Movement Module (stop movement)

### Empty Garbage Container
- UI Module (open garbage)

### Set Automatic Schedule
- UI Module
- Schedule Module

### View Robot Status
- UI Module
- Sensor Module (see status)

## Maintenance Person Use Cases
Only perform maintenance requires modules, as the robot is off otherwise
### Perform Maintenance
- UI Module (see advanced status)
- Sensor Module (status)

## Cleaning Robot Use Cases
### Navigate Canteen
- Navigation/Movement Module
- Sensor Module

### Detect Garbage
- Sensor Module

### Collect Trash
- Garbage Collection Module

### Alert When Full
- UI Module
- Sensor Module (detects if its full)

