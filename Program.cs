using RoverExcercise.Exceptions;
using RoverExcercise.Models;
using RoverExcercise.Services;
using System;
using System.Collections.Generic;

namespace RoverExcercise
{
    class Program
    {
        static List<string> userInput = new List<string>();
        List<ValidationErrors> validationErrors = new List<ValidationErrors>();
        static MarsGrid marsGrid = new MarsGrid();


        static void Main(string[] args)
        {

            ManageRovers();

        }

        private static void ManageRovers()
        {
            IRoverEngineService roverEngineService = new RoverEngineService();
            List<ValidationErrors> validationErrors = new List<ValidationErrors>();
            
            string roverCurrentPosition = string.Empty;
            string roverNavigationInstruction = string.Empty;
            List<RoverPosition> completedRoverPositions = new List<RoverPosition>();
            RoverPosition currentRoverPosition = new RoverPosition();

            Console.WriteLine("Please enter the boundaries of the grid followed by a line for the current position of the rover \n and a line for the exploration path the rover needs to follow. Once all the information has been entered please enter \\ to send instructions to the rovers.");

            string input = string.Empty;
            do
            {
                input = Console.ReadLine();
                userInput.Add(input);

            } while (input != "\\");

            if (!roverEngineService.ValidateUserInput(userInput, out validationErrors))
            {
                Console.WriteLine("The following errors occured in given input");
                foreach (var validationError in validationErrors)
                {
                    Console.WriteLine(string.Format("Line No.{0}:{1}",validationError.LineNo, validationError.ValidationMessage));
                }
                Console.ReadLine();
                return;
            }

            roverEngineService.SetGridBoundaries(marsGrid, userInput[0]);
            for(int index = 1; index < userInput.Count; index++)
            {
                if (index % 2 == 0)
                {
                    try
                    {
                        completedRoverPositions.Add(roverEngineService.NavigateRover(marsGrid, currentRoverPosition, userInput[index]));
                    }
                    catch(PlanetOutOfBoundsException ex)
                    {
                        Console.WriteLine("Error on Rover No. {0}.{1}", (index / 2), ex.Message);
                    }
                }
                else
                {
                    currentRoverPosition = roverEngineService.GetRoverPosition(userInput[index]);
                }
            }

            foreach(var item in completedRoverPositions)
            {
                Console.WriteLine("{0} {1} {2}", item.XPosition,item.YPosition,item.Direction);
            }

            Console.ReadLine();

        }
    }
}
