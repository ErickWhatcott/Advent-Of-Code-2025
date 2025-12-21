using System.Collections;
using CommunityToolkit.HighPerformance;

namespace AdventOfCode;

public static partial class Solution2023
{
    [DefineInput(InputType.FullInput)]
    public static (int, int) Day17(string? input = null)
    {
        input ??= ReadFullInput();
        return (Part1Day17(input), Part2Day17(input));
    }

    // Both of these implementations treats the graph as unweighted
    // by doing this it is able to run BFS on the graph.
    public static unsafe int Part1Day17(string input)
    {
        int width = input.IndexOf('\n');
        if (width == -1) width = input.Length;
        // Direction: 0 is up, 1 is right, 2 is down, 3 is left.
        fixed (char* c = input)
        {
            // The pitch makes it skip the newline character.
            Span2D<char> span = new(c, input.Length / width, width, 1);

            HashSet<(int x, int y, int current_line, int direction)> visited = [];
            Queue<(int x, int y, int heat_loss, int depth, int current_line, int direction)> queue = [];
            queue.Enqueue((0, 0, 0, span[0, 0], 0, 1));
            queue.Enqueue((0, 0, 0, span[0, 0], 0, 2));
            do
            {
                var curr = queue.Dequeue();
                if (curr.x == (span.Height - 1) && curr.y == (span.Width - 1))
                {
                    return curr.heat_loss - (span[0, 0] - '0') + (span[curr.x, curr.y] - '0');
                }
                else if (curr.depth == '1')
                {
                    if (!visited.Add((curr.x, curr.y, curr.current_line, curr.direction)))
                        continue;
                    switch (curr.direction)
                    {
                        // It's traveling up
                        case 0:
                            // One to the left,
                            if (curr.x > 0)
                                queue.Enqueue((curr.x - 1, curr.y, curr.heat_loss + 1, span[curr.x - 1, curr.y], 0, 3));
                            // One to the right,
                            if ((curr.x + 1) < span.Height)
                                queue.Enqueue((curr.x + 1, curr.y, curr.heat_loss + 1, span[curr.x + 1, curr.y], 0, 1));
                            // And maybe? one up.
                            if (curr.current_line < 2 && curr.y > 0)
                                queue.Enqueue((curr.x, curr.y - 1, curr.heat_loss + 1, span[curr.x, curr.y - 1], curr.current_line + 1, 0));
                            break;

                        // It's traveling right
                        case 1:
                            // Up
                            if (curr.y > 0)
                                queue.Enqueue((curr.x, curr.y - 1, curr.heat_loss + 1, span[curr.x, curr.y - 1], 0, 0));
                            // Down
                            if ((curr.y + 1) < span.Width)
                                queue.Enqueue((curr.x, curr.y + 1, curr.heat_loss + 1, span[curr.x, curr.y + 1], 0, 2));
                            // Right
                            if (curr.current_line < 2 && (curr.x + 1) < span.Height)
                                queue.Enqueue((curr.x + 1, curr.y, curr.heat_loss + 1, span[curr.x + 1, curr.y], curr.current_line + 1, 1));
                            break;

                        // It's traveling down
                        case 2:
                            // One to the left,
                            if (curr.x > 0)
                                queue.Enqueue((curr.x - 1, curr.y, curr.heat_loss + 1, span[curr.x - 1, curr.y], 0, 3));
                            // One to the right,
                            if ((curr.x + 1) < span.Height)
                                queue.Enqueue((curr.x + 1, curr.y, curr.heat_loss + 1, span[curr.x + 1, curr.y], 0, 1));
                            // And maybe? one down.
                            if (curr.current_line < 2 && (curr.y + 1) < span.Width)
                                queue.Enqueue((curr.x, curr.y + 1, curr.heat_loss + 1, span[curr.x, curr.y + 1], curr.current_line + 1, 2));
                            break;

                        // It's traveling left
                        case 3:
                            // Up
                            if (curr.y > 0)
                                queue.Enqueue((curr.x, curr.y - 1, curr.heat_loss + 1, span[curr.x, curr.y - 1], 0, 0));
                            // Down
                            if ((curr.y + 1) < span.Width)
                                queue.Enqueue((curr.x, curr.y + 1, curr.heat_loss + 1, span[curr.x, curr.y + 1], 0, 2));
                            // Left
                            if (curr.current_line < 2 && curr.x > 0)
                                queue.Enqueue((curr.x - 1, curr.y, curr.heat_loss + 1, span[curr.x - 1, curr.y], curr.current_line + 1, 3));
                            break;
                    }
                }
                else
                {
                    curr.depth--;
                    curr.heat_loss++;
                    queue.Enqueue(curr);
                }
            } while (queue.Count > 0);
        }

        return -1;
    }

    public static unsafe int Part2Day17(string input)
    {
        checked
        {
            int width = input.IndexOf('\n');
            if (width == -1) width = input.Length;
            // Direction: 0 is up, 1 is right, 2 is down, 3 is left.
            fixed (char* c = input)
            {
                // The pitch makes it skip the newline character.
                Span2D<char> span = new(c, input.Length / width, width, 1);

                HashSet<(int x, int y, int current_line, int direction)> visited = [];
                Queue<(int x, int y, int heat_loss, int depth, int current_line, int direction)> queue = [];
                queue.Enqueue((0, 0, 0, span[0, 0], 0, 1));
                queue.Enqueue((0, 0, 0, span[0, 0], 0, 2));
                do
                {
                    var curr = queue.Dequeue();
                    if (curr.x == (span.Height - 1) && curr.y == (span.Width - 1) && curr.current_line > 2)
                    {
                        return curr.heat_loss - (span[0, 0] - '0') + (span[curr.x, curr.y] - '0');
                    }
                    else if (curr.depth == '1')
                    {
                        if (!visited.Add((curr.x, curr.y, curr.current_line, curr.direction)))
                            continue;
                        switch (curr.direction)
                        {
                            // It's traveling up
                            case 0:
                                if (curr.current_line > 2)
                                {
                                    // One to the left,
                                    if (curr.x > 0)
                                        queue.Enqueue((curr.x - 1, curr.y, curr.heat_loss + 1, span[curr.x - 1, curr.y], 0, 3));
                                    // One to the right,
                                    if ((curr.x + 1) < span.Height)
                                        queue.Enqueue((curr.x + 1, curr.y, curr.heat_loss + 1, span[curr.x + 1, curr.y], 0, 1));
                                }
                                // And maybe? one up.
                                if (curr.current_line < 9 && curr.y > 0)
                                    queue.Enqueue((curr.x, curr.y - 1, curr.heat_loss + 1, span[curr.x, curr.y - 1], curr.current_line + 1, 0));
                                break;

                            // It's traveling right
                            case 1:
                                if (curr.current_line > 2)
                                {
                                    // Up
                                    if (curr.y > 0)
                                        queue.Enqueue((curr.x, curr.y - 1, curr.heat_loss + 1, span[curr.x, curr.y - 1], 0, 0));
                                    // Down
                                    if ((curr.y + 1) < span.Width)
                                        queue.Enqueue((curr.x, curr.y + 1, curr.heat_loss + 1, span[curr.x, curr.y + 1], 0, 2));
                                }

                                // Right
                                if (curr.current_line < 9 && (curr.x + 1) < span.Height)
                                    queue.Enqueue((curr.x + 1, curr.y, curr.heat_loss + 1, span[curr.x + 1, curr.y], curr.current_line + 1, 1));
                                break;

                            // It's traveling down
                            case 2:
                                if (curr.current_line > 2)
                                {
                                    // One to the left,
                                    if (curr.x > 0)
                                        queue.Enqueue((curr.x - 1, curr.y, curr.heat_loss + 1, span[curr.x - 1, curr.y], 0, 3));
                                    // One to the right,
                                    if ((curr.x + 1) < span.Height)
                                        queue.Enqueue((curr.x + 1, curr.y, curr.heat_loss + 1, span[curr.x + 1, curr.y], 0, 1));
                                }

                                // And maybe? one down.
                                if (curr.current_line < 9 && (curr.y + 1) < span.Width)
                                    queue.Enqueue((curr.x, curr.y + 1, curr.heat_loss + 1, span[curr.x, curr.y + 1], curr.current_line + 1, 2));
                                break;

                            // It's traveling left
                            case 3:
                                if (curr.current_line > 2)
                                {
                                    // Up
                                    if (curr.y > 0)
                                        queue.Enqueue((curr.x, curr.y - 1, curr.heat_loss + 1, span[curr.x, curr.y - 1], 0, 0));
                                    // Down
                                    if ((curr.y + 1) < span.Width)
                                        queue.Enqueue((curr.x, curr.y + 1, curr.heat_loss + 1, span[curr.x, curr.y + 1], 0, 2));
                                }

                                // Left
                                if (curr.current_line < 9 && curr.x > 0)
                                    queue.Enqueue((curr.x - 1, curr.y, curr.heat_loss + 1, span[curr.x - 1, curr.y], curr.current_line + 1, 3));
                                break;
                        }
                    }
                    else
                    {
                        curr.depth--;
                        curr.heat_loss++;
                        queue.Enqueue(curr);
                    }
                } while (queue.Count > 0);
            }

            return -1;
        }
    }
}