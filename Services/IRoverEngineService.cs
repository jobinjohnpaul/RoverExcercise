using RoverExcercise.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace RoverExcercise.Services
{
    public interface IRoverEngineService
    {
        void SetGridBoundaries(MarsGrid grid, string boundaryFeed);

        RoverPosition GetRoverPosition(string positionFeed);

        RoverPosition MoveRoverPosition(MarsGrid marsBoundaries, RoverPosition currentRoverPosition);

        RoverPosition TurnRoverDirection(RoverPosition currentRoverPosition, char navigationFeed);

        RoverPosition NavigateRover(MarsGrid marsBoundaries, RoverPosition currentRoverPosition, string navigationFeed);

        bool ValidateUserInput(List<string> userInput, out List<ValidationErrors> validationErrors);
    }
}
