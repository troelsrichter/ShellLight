using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using ShellLight.Contract;
using ShellLight.Contract.Attributes;

namespace ShellLight
{
  public static class CommandFinder
  {
    public static List<UICommand> Find(string searchText, List<UICommand> commands, out string parameter)
    {
      parameter = string.Empty;
      var criteria = searchText;
      var foundCommands = new List<UICommand>();

      while ((criteria.Length > 0) && (foundCommands.Count == 0))
      {

        var result = from c in commands
                     where c.Name.ToLower().Contains(criteria.ToLower())
                     select c;

        foundCommands = result.ToList();

        if (foundCommands.Count == 0)
        {
          if (criteria.Contains(" "))
          {
            var parts = criteria.Split(' ');
            var lengthOfLastPart = parts[parts.Length - 1].Length;
            criteria = criteria.Substring(0, criteria.Length - lengthOfLastPart - 1);

          }
          else
          {
            //no more to search for
            criteria = string.Empty;
          }
        }

        if ((foundCommands.Count == 1) && (criteria.Length < searchText.Length))
        {
          parameter = searchText.Substring(criteria.Length + 1);
        }

      }
      return foundCommands;
    }

    public static List<UICommand> CreateTop3Commands(IEnumerable<UICommand> commands)
    {
      var topScoreCommands = new List<UICommand>();
      var result = from c in commands where c.Score > 0 orderby c.Score descending select c;
      if (result.Count() > 0)
      {
        topScoreCommands = result.Take(3).ToList();
      }
      return topScoreCommands;
    }

    public static ObservableCollection<UICommand> FilterCommands(IEnumerable<UICommand> commands)
    {
        //Filter hidden commands
        var result = from c in commands
                     where
                         !(c.HasAttribute<LauncherAttribute>() &&
                           c.GetAttribute<LauncherAttribute>().VisibilityType == VisibilityType.Hidden)
                     select c;
        return new ObservableCollection<UICommand>(result);
    }

  }
}