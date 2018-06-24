using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using RoverExcercise.Exceptions;
using RoverExcercise.Models;

namespace RoverExcercise.Services
{
    public class RoverEngineService : IRoverEngineService
    {
        public void SetGridBoundaries(MarsGrid marsGrid, string boundaryFeed)
        {
            var boundaryFeedSplit = boundaryFeed.Trim().Split(" ");
            marsGrid.XMaxBoundary = int.Parse(boundaryFeedSplit[0].ToString());
            marsGrid.YMaxBoundary = int.Parse(boundaryFeedSplit[1].ToString());
        }

        public RoverPosition GetRoverPosition(string positionFeed)
        {
            var positionFeedSplit = positionFeed.Trim().Split(" ");
            var roverPosition = new RoverPosition();
            roverPosition.XPosition = int.Parse(positionFeedSplit[0].ToString());
            roverPosition.YPosition = int.Parse(positionFeedSplit[1].ToString());
            roverPosition.Direction = positionFeedSplit[2].ToString();

            return roverPosition;
        }

        
        public RoverPosition MoveRoverPosition(MarsGrid marsGrid, RoverPosition currentRoverPosition)
        {
            int newYPos = 0;
            int newXPos = 0;
            string exceptionMsg = "Cannot move forward, end of planet reached";

            switch (currentRoverPosition.Direction)
            {
                case "N":
                    newYPos = currentRoverPosition.YPosition + 1;
                    if(newYPos <= marsGrid.YMaxBoundary )
                    {
                        currentRoverPosition.YPosition = newYPos;
                    }
                    else
                    {
                        throw new PlanetOutOfBoundsException(exceptionMsg);
                    }
                    break;

                case "W":
                    newXPos = currentRoverPosition.XPosition - 1;
                    if (newXPos >= marsGrid.XMinBoundary)
                    {
                        currentRoverPosition.XPosition = newXPos;
                    }
                    else
                    {
                        throw new PlanetOutOfBoundsException(exceptionMsg);
                    }
                    break;

                case "S":
                    newYPos = currentRoverPosition.YPosition - 1;
                    if (newYPos >= marsGrid.YMinBoundary)
                    {
                        currentRoverPosition.YPosition = newYPos;
                    }
                    else
                    {
                        throw new PlanetOutOfBoundsException(exceptionMsg);
                    }
                    break;

                case "E":
                    newXPos = currentRoverPosition.XPosition + 1;
                    if (newXPos <= marsGrid.XMaxBoundary)
                    {
                        currentRoverPosition.XPosition = newXPos;
                    }
                    else
                    {
                        throw new Exception(exceptionMsg);
                    }
                    break;
            }

            
            return currentRoverPosition;
        }

        public RoverPosition NavigateRover(MarsGrid marsGrid, RoverPosition currentRoverPosition, string navigationFeed)
        {
            foreach (var navigationCmd in navigationFeed.ToUpper())
            {
                switch (navigationCmd)
                {
                    case 'M':
                        MoveRoverPosition(marsGrid, currentRoverPosition);
                        break;
                    case 'L':
                    case 'R':
                        TurnRoverDirection(currentRoverPosition, navigationCmd);
                        break;
                }
            }

            return currentRoverPosition;
        }

        public RoverPosition TurnRoverDirection(RoverPosition currentRoverPosition, char directionFeed)
        {
            switch (directionFeed)
            {
                case 'L':
                    switch (currentRoverPosition.Direction.ToUpper())
                    {
                        case "N":
                            currentRoverPosition.Direction = "W";
                            break;
                        case "W":
                            currentRoverPosition.Direction = "S";
                            break;
                        case "S":
                            currentRoverPosition.Direction = "E";
                            break;
                        case "E":
                            currentRoverPosition.Direction = "N";
                            break;                        
                    }
                    break;

                case 'R':
                    switch (currentRoverPosition.Direction.ToUpper())
                    {
                        case "N":
                            currentRoverPosition.Direction = "E";
                            break;
                        case "E":
                            currentRoverPosition.Direction = "S";
                            break;
                        case "S":
                            currentRoverPosition.Direction = "W";
                            break;
                        case "W":
                            currentRoverPosition.Direction = "N";
                            break;
                    }
                    break;
            }

            return currentRoverPosition;
        }

        public bool ValidateUserInput(List<string> userInput, out List<ValidationErrors> validationErrors)
        {
            int outputCoordVar = 0;
            string outputDirVar = string.Empty;

            validationErrors = new List<ValidationErrors>();
            userInput.Remove("\\");
            if(userInput.Count == 0)
            {
                validationErrors.Add(new ValidationErrors() { LineNo = 0, ValidationMessage = "No user input received." });
                return false;
            }
            if (userInput.Count == 1)
            {
                validationErrors.Add(new ValidationErrors() { LineNo = 0, ValidationMessage = "No rover information received." });
                return false;
            }
            var planetBoundaries = userInput[0];
            var planetBoundariesSplit = planetBoundaries.Trim().Split(" ");
            
            if (planetBoundariesSplit.Length < 2)
            {
                validationErrors.Add(new ValidationErrors() { LineNo = 1, ValidationMessage = "Planet upper boundary requires both X and Y coordinates." });
                return false;
            }

            if (userInput.Count % 2 == 0)
            {
                validationErrors.Add(new ValidationErrors() { LineNo = 0, ValidationMessage = "Invalid number of lines for rover navigation." });
                return false;
            }

            for(var counter = 1; counter < userInput.Count; counter++)
            {
                if(counter % 2 != 0)
                {
                    var roverPosition = userInput[counter];
                    var roverPositionSplit = roverPosition.Trim().Split(" ");
                    if (roverPositionSplit.Length < 3)
                    {
                        validationErrors.Add(new ValidationErrors() { LineNo = counter, ValidationMessage = "Rover position requires X,Y coordinates and direction." });
                        return false;
                    }
                    if (!int.TryParse(roverPositionSplit[0], out outputCoordVar))
                    {
                        validationErrors.Add(new ValidationErrors() { LineNo = (counter + 1) / 2, ValidationMessage = "Rover position requires valid integer X coordinate." });
                        return false;
                    }
                    if (!int.TryParse(roverPositionSplit[1], out outputCoordVar))
                    {
                        validationErrors.Add(new ValidationErrors() { LineNo = (counter + 1) / 2, ValidationMessage = "Rover position requires valid integer Y coordinate." });
                        return false;
                    }
                    if (!Regex.IsMatch(roverPositionSplit[2].ToUpper(), "[NSEW]"))
                    {
                        validationErrors.Add(new ValidationErrors() { LineNo = (counter + 1) / 2, ValidationMessage = "Rover position requires valid direction." });
                        return false;
                    }
                }
                else
                {
                    if (!Regex.IsMatch(userInput[counter].ToUpper(), "[LRM]"))
                    {
                        validationErrors.Add(new ValidationErrors() { LineNo = counter , ValidationMessage = "Rover directions should be either L R or M." });
                        return false;
                    }
                }
            }

            return true;
        }
    }
}
