// Decompiled with JetBrains decompiler
// Type: GameStagesFromXml
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System.Collections;
using System.Collections.Generic;
using System.Xml.Linq;

#nullable disable
public class GameStagesFromXml
{
  [PublicizedFrom(EAccessModifier.Private)]
  public const string XMLName = "gamestages.xml";

  public static IEnumerator Load(XmlFile _xmlFile)
  {
    GameStageGroup.Clear();
    List<GameStagesFromXml.Group> groupList = new List<GameStagesFromXml.Group>();
    XElement root = _xmlFile.XmlDoc.Root;
    if (!root.HasElements)
      throw new XmlLoadException("gamestages.xml", root, "Missing root element!");
    foreach (XElement element in root.Elements())
    {
      if (element.Name == (XName) "spawner")
        GameStagesFromXml.ParseGameStageDef(element);
      else if (element.Name == (XName) "group")
      {
        GameStagesFromXml.Group gameStageGroup = GameStagesFromXml.ParseGameStageGroup(element);
        groupList.Add(gameStageGroup);
      }
      else if (element.Name == (XName) "config")
      {
        if (element.HasAttribute((XName) "startingWeight"))
          GameStageDefinition.StartingWeight = StringParsers.ParseFloat(element.GetAttribute((XName) "startingWeight"));
        if (element.HasAttribute((XName) "difficultyBonus"))
          GameStageDefinition.DifficultyBonus = StringParsers.ParseFloat(element.GetAttribute((XName) "difficultyBonus"));
        if (element.HasAttribute((XName) "daysAliveChangeWhenKilled"))
          GameStageDefinition.DaysAliveChangeWhenKilled = long.Parse(element.GetAttribute((XName) "daysAliveChangeWhenKilled"));
        if (element.HasAttribute((XName) "diminishingReturns"))
          GameStageDefinition.DiminishingReturns = StringParsers.ParseFloat(element.GetAttribute((XName) "diminishingReturns"));
        if (element.HasAttribute((XName) "lootBonusEvery"))
          GameStageDefinition.LootBonusEvery = int.Parse(element.GetAttribute((XName) "lootBonusEvery"));
        if (element.HasAttribute((XName) "lootBonusMaxCount"))
          GameStageDefinition.LootBonusMaxCount = int.Parse(element.GetAttribute((XName) "lootBonusMaxCount"));
        if (element.HasAttribute((XName) "lootBonusScale"))
          GameStageDefinition.LootBonusScale = StringParsers.ParseFloat(element.GetAttribute((XName) "lootBonusScale"));
        string attribute1;
        if ((attribute1 = element.GetAttribute((XName) "lootWanderingBonusEvery")).Length > 0)
          GameStageDefinition.LootWanderingBonusEvery = int.Parse(attribute1);
        string attribute2;
        if ((attribute2 = element.GetAttribute((XName) "lootWanderingBonusScale")).Length > 0)
          GameStageDefinition.LootWanderingBonusScale = StringParsers.ParseFloat(attribute2);
      }
    }
    for (int index = 0; index < groupList.Count; ++index)
    {
      GameStagesFromXml.Group group = groupList[index];
      string name = group.spawnerName;
      if (string.IsNullOrEmpty(name))
        name = "SleeperGSList";
      GameStageDefinition definition;
      if (!GameStageDefinition.TryGetGameStage(name, out definition))
        throw new XmlLoadException("gamestages.xml", group.element, $"Group '{group.name}': Spawner '{name}' not found!");
      GameStageGroup _group = new GameStageGroup(definition);
      GameStageGroup.AddGameStageGroup(group.name, _group);
    }
    yield break;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static GameStagesFromXml.Group ParseGameStageGroup(XElement root)
  {
    string attribute1 = root.GetAttribute((XName) "name");
    if (attribute1.Length == 0)
      throw new XmlLoadException("gamestages.xml", root, "<group> missing name!");
    string attribute2 = root.GetAttribute((XName) "spawner");
    return new GameStagesFromXml.Group(attribute1, attribute2, root);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void ParseGameStageDef(XElement root)
  {
    string attribute = root.GetAttribute((XName) "name");
    GameStageDefinition gameStageDefinition = attribute.Length != 0 ? new GameStageDefinition(attribute) : throw new XmlLoadException("gamestages.xml", root, "<spawner> missing name!");
    foreach (XElement element in root.Elements((XName) "gamestage"))
      GameStagesFromXml.ParseStage(gameStageDefinition, element);
    GameStageDefinition.AddGameStage(gameStageDefinition);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void ParseStage(GameStageDefinition gsd, XElement root)
  {
    string attribute = root.GetAttribute((XName) "stage");
    GameStageDefinition.Stage stage = attribute.Length != 0 ? new GameStageDefinition.Stage(int.Parse(attribute)) : throw new XmlLoadException("gamestages.xml", root, $"GameStage {gsd.name} sub element is missing stage!");
    foreach (XElement element in root.Elements((XName) "spawn"))
      GameStagesFromXml.ParseSpawn(gsd, stage, element);
    if (stage.Count <= 0)
      return;
    gsd.AddStage(stage);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public static void ParseSpawn(
    GameStageDefinition gsd,
    GameStageDefinition.Stage stage,
    XElement root)
  {
    string attribute = root.GetAttribute((XName) "group");
    if (attribute.Length == 0)
      throw new XmlLoadException("gamestages.xml", root, "<spawn> is missing group!");
    if (!EntityGroups.list.ContainsKey(attribute))
      throw new XmlLoadException("gamestages.xml", root, $"Spawner '{gsd.name}', gamestage {stage.stageNum}: EntityGroup '{attribute}' unknown!");
    int _result1 = 1;
    root.ParseAttribute((XName) "num", ref _result1);
    int _result2 = 1;
    root.ParseAttribute((XName) "maxAlive", ref _result2);
    float _result3 = 0.0f;
    root.ParseAttribute((XName) "interval", ref _result3);
    ulong _result4 = 1;
    root.ParseAttribute((XName) "duration", ref _result4);
    GameStageDefinition.SpawnGroup spawn = new GameStageDefinition.SpawnGroup(attribute, _result1, _result2, _result3, _result4);
    stage.AddSpawnGroup(spawn);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public struct Group(string _name, string _spawnerName, XElement _element)
  {
    public readonly string name = _name;
    public readonly string spawnerName = _spawnerName;
    public readonly XElement element = _element;
  }
}
