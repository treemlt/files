// Decompiled with JetBrains decompiler
// Type: SelectionBox
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: EC45F2DF-A0B8-4F90-B2AA-EDFF3B83B22C
// Assembly location: C:\Users\TreeMT\Desktop\Assembly-CSharp.dll

using System;
using System.Collections.Generic;
using UnityEngine;

#nullable disable
public class SelectionBox : MonoBehaviour
{
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const float boundsPadding = 0.16f;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public const int maxFacingDirectionDistance = 62500;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly int zTestShaderProperty = Shader.PropertyToID("_ZTest");
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly int colorShaderProperty = Shader.PropertyToID("_Color");
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public SelectionCategory ownerCategory;
  public object UserData;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public GameObject frame;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public TextMesh captionMesh;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public TextMesh[] sizeMeshes;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3i size = Vector3i.zero;
  public Bounds bounds = BoundsUtils.BoundsForMinMax(Vector3.zero, Vector3.one);
  public RenderCubeType focusType = RenderCubeType.FullBlockBothSides;
  public bool bAlwaysDrawDirection;
  public bool bDrawDirection;
  public float facingDirection;
  public Vector3 AxisOrigin;
  public readonly List<Vector3> Axises = new List<Vector3>();
  public readonly List<Vector3i> AxisesI = new List<Vector3i>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Vector3i hightlightedAxis = Vector3i.zero;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public MeshFilter m_MeshFilter;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public MeshRenderer m_MeshRenderer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<int>[] subMeshTriangles = new List<int>[6];
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<Vector2> m_Uvs = new List<Vector2>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly List<Vector3> m_Vertices = new List<Vector3>();
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly Material[] materialsArr = new Material[6];
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int collLayer;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public string collTag;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public int currentChunkMeshIndex;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool bCreated;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public readonly bool[] faceColorsSet = new bool[6];
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Color curColor;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public Color curFrameColor = SelectionBox.inActiveFrameColor;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public bool curShowingThroughWalls;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly Color inActiveFrameColor = Color.blue;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly Color activeFrameColor = Color.green;
  [PublicizedFrom(EAccessModifier.Private)]
  [NonSerialized]
  public static readonly SelectionBox.SizeTextMeshDefinition[] SizeTextMeshDefs = new SelectionBox.SizeTextMeshDefinition[5]
  {
    new SelectionBox.SizeTextMeshDefinition("Top", Vector3.op_Addition(Vector3.up, new Vector3(0.0f, 0.0f, 0.2f)), new Vector3(90f, 0.0f, 0.0f), new char[3]
    {
      '↔',
      '↗',
      '↕'
    }),
    new SelectionBox.SizeTextMeshDefinition("Front", Vector3.op_Addition(Vector3.op_Multiply(Vector3.back, 0.5f), Vector3.op_Multiply(Vector3.up, 0.5f)), new Vector3(0.0f, 0.0f, 0.0f), new char[3]
    {
      '↔',
      '↕',
      '↗'
    }),
    new SelectionBox.SizeTextMeshDefinition("Back", Vector3.op_Addition(Vector3.op_Multiply(Vector3.forward, 0.5f), Vector3.op_Multiply(Vector3.up, 0.5f)), new Vector3(0.0f, 180f, 0.0f), new char[3]
    {
      '↔',
      '↕',
      '↗'
    }),
    new SelectionBox.SizeTextMeshDefinition("Left", Vector3.op_Addition(Vector3.op_Multiply(Vector3.left, 0.5f), Vector3.op_Multiply(Vector3.up, 0.5f)), new Vector3(0.0f, 90f, 0.0f), new char[3]
    {
      '↗',
      '↕',
      '↔'
    }),
    new SelectionBox.SizeTextMeshDefinition("Right", Vector3.op_Addition(Vector3.op_Multiply(Vector3.right, 0.5f), Vector3.op_Multiply(Vector3.up, 0.5f)), new Vector3(0.0f, -90f, 0.0f), new char[3]
    {
      '↗',
      '↕',
      '↔'
    })
  };

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Awake()
  {
    GameObject gameObject = new GameObject("Box");
    gameObject.transform.parent = ((Component) this).transform;
    gameObject.transform.localPosition = Vector3.zero;
    gameObject.transform.localScale = ((Bounds) ref this.bounds).size;
    this.m_MeshFilter = gameObject.AddComponent<MeshFilter>();
    this.m_MeshRenderer = gameObject.AddComponent<MeshRenderer>();
    for (int index = 0; index < this.materialsArr.Length; ++index)
    {
      this.materialsArr[index] = new Material(Resources.Load<Shader>("Shaders/SelectionBox"));
      this.materialsArr[index].renderQueue = -1;
    }
    ((Renderer) this.m_MeshRenderer).materials = this.materialsArr;
    for (int index = 0; index < this.subMeshTriangles.Length; ++index)
      this.subMeshTriangles[index] = new List<int>();
    this.ResetAllFacesColor();
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void Start() => Camera.onPostRender += new Camera.CameraCallback(this.camPostRender);

  [PublicizedFrom(EAccessModifier.Private)]
  public void OnDestroy()
  {
    Camera.onPostRender -= new Camera.CameraCallback(this.camPostRender);
    Utils.CleanupMaterials<Material[]>(this.materialsArr);
    Utils.CleanupMaterialsOfRenderer((Renderer) this.m_MeshRenderer);
    if (!Object.op_Inequality((Object) this.frame, (Object) null))
      return;
    Object.Destroy((Object) this.frame);
  }

  public void SetOwner(SelectionCategory _selectionCategory)
  {
    this.ownerCategory = _selectionCategory;
  }

  public void SetAllFacesColor(Color _c, bool useAlphaMultiplier = true)
  {
    if (useAlphaMultiplier)
      _c.a *= SelectionBoxManager.Instance.AlphaMultiplier;
    if (Object.op_Inequality((Object) this.m_MeshRenderer, (Object) null))
    {
      foreach (Material material in ((Renderer) this.m_MeshRenderer).materials)
        material.color = _c;
    }
    this.curColor = _c;
  }

  public void ResetAllFacesColor() => this.SetAllFacesColor(this.curColor, false);

  public void SetFaceColor(BlockFace _face, Color _c)
  {
    ((Renderer) this.m_MeshRenderer).materials[(int) _face].color = _c;
    this.faceColorsSet[(int) _face] = true;
  }

  public void SetCaption(string _text)
  {
    if (Object.op_Equality((Object) this.captionMesh, (Object) null))
    {
      GameObject gameObject = new GameObject("Caption")
      {
        transform = {
          parent = ((Component) this).transform,
          localScale = Vector3.one,
          localPosition = Vector3.zero,
          rotation = Quaternion.Euler(90f, 0.0f, 0.0f)
        }
      };
      gameObject.transform.localPosition = new Vector3(0.0f, ((Bounds) ref this.bounds).size.y + 0.1f, 0.0f);
      this.captionMesh = NGUITools.AddMissingComponent<TextMesh>(gameObject);
      this.captionMesh.alignment = (TextAlignment) 1;
      this.captionMesh.anchor = (TextAnchor) 4;
      this.captionMesh.fontSize = 20;
      this.captionMesh.color = Color.green;
      gameObject.SetActive(true);
    }
    this.captionMesh.text = _text;
  }

  public void SetCaptionVisibility(bool _visible)
  {
    if (Object.op_Equality((Object) this.captionMesh, (Object) null))
      return;
    ((Component) this.captionMesh).gameObject.SetActive(_visible);
  }

  public void SetPositionAndSize(Vector3 _pos, Vector3i _size)
  {
    ((Component) this).transform.localPosition = Vector3.op_Subtraction(Vector3.op_Addition(_pos, new Vector3((float) _size.x * 0.5f, -0.1f, (float) _size.z * 0.5f)), Origin.position);
    this.bounds = BoundsUtils.BoundsForMinMax(_pos, Vector3.op_Addition(_pos, (Vector3) _size));
    ref Bounds local = ref this.bounds;
    ((Bounds) ref local).size = Vector3.op_Addition(((Bounds) ref local).size, new Vector3(0.16f, 0.16f, 0.16f));
    Transform boxTransform = this.GetBoxTransform();
    if (Object.op_Inequality((Object) boxTransform, (Object) null))
      boxTransform.localScale = ((Bounds) ref this.bounds).size;
    if (this.size != _size)
    {
      this.BuildFrame();
      if (Object.op_Inequality((Object) this.captionMesh, (Object) null))
        ((Component) this.captionMesh).transform.localPosition = new Vector3(0.0f, ((Bounds) ref this.bounds).size.y + 0.1f, 0.0f);
      this.UpdateSizeMeshes(_size);
    }
    this.size = _size;
  }

  public void SetVisible(bool _visible)
  {
    if (((Component) this).gameObject.activeSelf == _visible)
      return;
    ((Component) this).gameObject.SetActive(_visible);
    if (_visible)
    {
      this.ResetAllFacesColor();
      this.SetFrameActive(false);
    }
    switch (this.ownerCategory.name)
    {
      case "SleeperVolume":
        SleeperVolumeToolManager.ShowSleepers(_visible);
        break;
      case "POIMarker":
        POIMarkerToolManager.ShowPOIMarkers(_visible);
        break;
    }
  }

  public Vector3i GetScale() => new Vector3i(((Bounds) ref this.bounds).size);

  public Transform GetBoxTransform() => ((Component) this).transform.Find("Box");

  public void SetFrameActive(bool _active)
  {
    this.SetFrameColor(_active ? SelectionBox.activeFrameColor : SelectionBox.inActiveFrameColor);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void SetFrameColor(Color _color)
  {
    this.curFrameColor = _color;
    if (!Object.op_Inequality((Object) this.frame, (Object) null))
      return;
    ((Renderer) this.frame.GetComponent<MeshRenderer>()).material.SetColor(SelectionBox.colorShaderProperty, _color);
  }

  public void ShowThroughWalls(bool _bShow)
  {
    if (this.curShowingThroughWalls == _bShow)
      return;
    this.curShowingThroughWalls = _bShow;
    int num = _bShow ? 8 : 4;
    if (Object.op_Inequality((Object) this.frame, (Object) null))
      ((Renderer) this.frame.GetComponent<MeshRenderer>()).material.SetInt(SelectionBox.zTestShaderProperty, num);
    foreach (Material material in ((Renderer) this.m_MeshRenderer).materials)
      material.SetInt(SelectionBox.zTestShaderProperty, num);
  }

  public void EnableCollider(string _tag, int _layer)
  {
    this.collLayer = _layer;
    this.collTag = _tag;
  }

  public void HighlightAxis(Vector3i _hightlightedAxis)
  {
    this.hightlightedAxis = _hightlightedAxis;
  }

  [PublicizedFrom(EAccessModifier.Protected)]
  public void Update()
  {
    if (this.bCreated || !this.createMesh())
      return;
    this.bCreated = true;
  }

  public void SetSizeVisibility(bool _visible)
  {
    if (_visible && this.sizeMeshes == null)
    {
      this.sizeMeshes = new TextMesh[SelectionBox.SizeTextMeshDefs.Length];
      for (int index = 0; index < this.sizeMeshes.Length; ++index)
      {
        SelectionBox.SizeTextMeshDefinition sizeTextMeshDef = SelectionBox.SizeTextMeshDefs[index];
        GameObject gameObject = new GameObject("Size_" + sizeTextMeshDef.Name);
        gameObject.transform.parent = ((Component) this).transform;
        gameObject.transform.localScale = Vector3.one;
        gameObject.transform.localPosition = Vector3.Scale(((Bounds) ref this.bounds).size, sizeTextMeshDef.Position);
        gameObject.transform.rotation = Quaternion.Euler(sizeTextMeshDef.Rotation);
        TextMesh textMesh = NGUITools.AddMissingComponent<TextMesh>(gameObject);
        this.sizeMeshes[index] = textMesh;
        textMesh.alignment = (TextAlignment) 1;
        textMesh.anchor = (TextAnchor) 4;
        textMesh.characterSize = 0.1f;
        textMesh.fontSize = 20;
        textMesh.color = Color.green;
        textMesh.text = sizeTextMeshDef.Name;
        gameObject.SetActive(true);
      }
      this.UpdateSizeMeshes(this.size);
    }
    if (this.sizeMeshes == null)
      return;
    foreach (Component sizeMesh in this.sizeMeshes)
      sizeMesh.gameObject.SetActive(_visible);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void UpdateSizeMeshes(Vector3i _newSize)
  {
    if (this.sizeMeshes == null)
      return;
    for (int index = 0; index < this.sizeMeshes.Length; ++index)
    {
      SelectionBox.SizeTextMeshDefinition sizeTextMeshDef = SelectionBox.SizeTextMeshDefs[index];
      this.sizeMeshes[index].text = $"{sizeTextMeshDef.Arrows[0]}{_newSize.x} {sizeTextMeshDef.Arrows[1]}{_newSize.y} {sizeTextMeshDef.Arrows[2]}{_newSize.z}";
      ((Component) this.sizeMeshes[index]).transform.localPosition = Vector3.Scale(((Bounds) ref this.bounds).size, sizeTextMeshDef.Position);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void camPostRender(Camera _cam)
  {
    if (Object.op_Inequality((Object) _cam, (Object) Camera.main))
      return;
    Vector3 vector3_1 = Vector3.op_Subtraction(GameManager.Instance.World.GetPrimaryPlayer().position, ((Bounds) ref this.bounds).center);
    float sqrMagnitude = ((Vector3) ref vector3_1).sqrMagnitude;
    (SelectionCategory, SelectionBox)? selection = SelectionBoxManager.Instance.Selection;
    ref (SelectionCategory, SelectionBox)? local = ref selection;
    bool flag = Object.op_Equality(local.HasValue ? (Object) local.GetValueOrDefault().Item2 : (Object) null, (Object) this);
    if (!flag && (double) sqrMagnitude > 62500.0 || !((Component) this).gameObject.activeInHierarchy)
      return;
    GUIUtils.SetupLines(_cam, 3f);
    if (this.bDrawDirection && (flag || this.bAlwaysDrawDirection))
    {
      Vector3 vector3_2 = Vector3.op_Addition(((Component) this).transform.position, new Vector3(0.0f, ((Bounds) ref this.bounds).size.y, 0.0f));
      float num1 = Mathf.Min(15f, 4f * Mathf.Max(((Bounds) ref this.bounds).size.x, ((Bounds) ref this.bounds).size.z));
      Vector3 vector3_3 = Vector3.op_Subtraction(((Component) _cam).transform.position, vector3_2);
      float num2 = Mathf.Clamp(((Vector3) ref vector3_3).magnitude / 10f, 1f, num1);
      Vector3 facing = Quaternion.op_Multiply(Quaternion.AngleAxis(this.facingDirection, Vector3.up), Vector3.forward);
      GUIUtils.DrawTriangleWide(Vector3.op_Addition(vector3_2, Vector3.op_Multiply(Vector3.op_Multiply(facing, 0.5f), Math.Min(((Bounds) ref this.bounds).size.x - 1f, ((Bounds) ref this.bounds).size.z - 1f))), facing, Vector3.up, num2 * 0.5f, Color.black);
    }
    if (!flag || GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 0)
      return;
    Vector3 vector3_4 = Vector3.op_Addition(((Component) this).transform.position, new Vector3(0.0f, ((Bounds) ref this.bounds).size.y * 0.5f, 0.0f));
    this.AxisOrigin = vector3_4;
    Vector3 vector3_5 = Vector3.op_Subtraction(((Component) _cam).transform.position, vector3_4);
    float num = Mathf.Max(1f, (float) ((double) ((Vector3) ref vector3_5).magnitude / 10.0));
    Color colorA1;
    // ISSUE: explicit constructor call
    ((Color) ref colorA1).\u002Ector(0.3f, 0.05f, 0.05f);
    Color color1;
    // ISSUE: explicit constructor call
    ((Color) ref color1).\u002Ector(1f, 0.6f, 0.6f);
    Color colorA2;
    // ISSUE: explicit constructor call
    ((Color) ref colorA2).\u002Ector(0.05f, 0.2f, 0.05f);
    Color color2;
    // ISSUE: explicit constructor call
    ((Color) ref color2).\u002Ector(0.0f, 0.7f, 0.0f);
    Color color3;
    // ISSUE: explicit constructor call
    ((Color) ref color3).\u002Ector(0.6f, 1f, 0.6f);
    Color colorA3;
    // ISSUE: explicit constructor call
    ((Color) ref colorA3).\u002Ector(0.05f, 0.05f, 0.3f);
    Color color4;
    // ISSUE: explicit constructor call
    ((Color) ref color4).\u002Ector(0.4f, 0.4f, 1f);
    Color color5;
    // ISSUE: explicit constructor call
    ((Color) ref color5).\u002Ector(0.7f, 0.7f, 1f);
    this.Axises.Clear();
    this.AxisesI.Clear();
    if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 1)
    {
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.right)));
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.up)));
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.forward)));
      this.AxisesI.Add(Vector3i.right);
      this.AxisesI.Add(Vector3i.up);
      this.AxisesI.Add(Vector3i.forward);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[0], colorA1, this.hightlightedAxis.x != 0 ? color1 : Color.red);
      Vector3 axise1 = this.Axises[0];
      Vector3 vector3_6 = Vector3.op_Subtraction(this.Axises[0], this.AxisOrigin);
      Vector3 normalized1 = ((Vector3) ref vector3_6).normalized;
      Vector3 up1 = Vector3.up;
      double size1 = (double) num * 0.125;
      Color _color1 = this.hightlightedAxis.x != 0 ? Color.yellow : Color.red;
      GUIUtils.DrawTriangleWide(axise1, normalized1, up1, (float) size1, _color1);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[1], colorA2, this.hightlightedAxis.y != 0 ? color3 : color2);
      Vector3 axise2 = this.Axises[1];
      Vector3 vector3_7 = Vector3.op_Subtraction(this.Axises[1], this.AxisOrigin);
      Vector3 normalized2 = ((Vector3) ref vector3_7).normalized;
      Vector3 right = Vector3.right;
      double size2 = (double) num * 0.125;
      Color _color2 = this.hightlightedAxis.y != 0 ? Color.yellow : color2;
      GUIUtils.DrawTriangleWide(axise2, normalized2, right, (float) size2, _color2);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[2], colorA3, this.hightlightedAxis.z != 0 ? color5 : color4);
      Vector3 axise3 = this.Axises[2];
      Vector3 vector3_8 = Vector3.op_Subtraction(this.Axises[2], this.AxisOrigin);
      Vector3 normalized3 = ((Vector3) ref vector3_8).normalized;
      Vector3 up2 = Vector3.up;
      double size3 = (double) num * 0.125;
      Color _color3 = this.hightlightedAxis.z != 0 ? Color.yellow : color4;
      GUIUtils.DrawTriangleWide(axise3, normalized3, up2, (float) size3, _color3);
    }
    else if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) == 2)
    {
      if (!this.ownerCategory.callback.OnSelectionBoxIsAvailable(this.ownerCategory.name, EnumSelectionBoxAvailabilities.CanResize))
        return;
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.right)));
      this.Axises.Add(Vector3.op_Subtraction(vector3_4, Vector3.op_Multiply(num, Vector3.right)));
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.up)));
      this.Axises.Add(Vector3.op_Subtraction(vector3_4, Vector3.op_Multiply(num, Vector3.up)));
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.forward)));
      this.Axises.Add(Vector3.op_Subtraction(vector3_4, Vector3.op_Multiply(num, Vector3.forward)));
      this.Axises.Add(vector3_4);
      this.AxisesI.Add(Vector3i.right);
      this.AxisesI.Add(Vector3i.left);
      this.AxisesI.Add(Vector3i.up);
      this.AxisesI.Add(Vector3i.down);
      this.AxisesI.Add(Vector3i.forward);
      this.AxisesI.Add(Vector3i.back);
      this.AxisesI.Add(Vector3i.one);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[0], colorA1, this.hightlightedAxis.x > 0 ? color1 : Color.red);
      Vector3 axise4 = this.Axises[0];
      Vector3 vector3_9 = Vector3.op_Subtraction(this.Axises[0], this.AxisOrigin);
      Vector3 normalized4 = ((Vector3) ref vector3_9).normalized;
      Vector3 up3 = Vector3.up;
      double size4 = (double) num * 0.125;
      Color _color4 = this.hightlightedAxis.x > 0 ? Color.yellow : Color.red;
      GUIUtils.DrawRectWide(axise4, normalized4, up3, (float) size4, _color4);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[1], colorA1, this.hightlightedAxis.x < 0 ? color1 : Color.red);
      Vector3 axise5 = this.Axises[1];
      Vector3 vector3_10 = Vector3.op_Subtraction(this.Axises[1], this.AxisOrigin);
      Vector3 normalized5 = ((Vector3) ref vector3_10).normalized;
      Vector3 up4 = Vector3.up;
      double size5 = (double) num * 0.125;
      Color _color5 = this.hightlightedAxis.x < 0 ? Color.yellow : Color.red;
      GUIUtils.DrawRectWide(axise5, normalized5, up4, (float) size5, _color5);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[2], colorA2, this.hightlightedAxis.y > 0 ? color3 : color2);
      Vector3 axise6 = this.Axises[2];
      Vector3 vector3_11 = Vector3.op_Subtraction(this.Axises[2], this.AxisOrigin);
      Vector3 normalized6 = ((Vector3) ref vector3_11).normalized;
      Vector3 right1 = Vector3.right;
      double size6 = (double) num * 0.125;
      Color _color6 = this.hightlightedAxis.y > 0 ? Color.yellow : color2;
      GUIUtils.DrawRectWide(axise6, normalized6, right1, (float) size6, _color6);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[3], colorA2, this.hightlightedAxis.y < 0 ? color3 : color2);
      Vector3 axise7 = this.Axises[3];
      Vector3 vector3_12 = Vector3.op_Subtraction(this.Axises[3], this.AxisOrigin);
      Vector3 normalized7 = ((Vector3) ref vector3_12).normalized;
      Vector3 right2 = Vector3.right;
      double size7 = (double) num * 0.125;
      Color _color7 = this.hightlightedAxis.y < 0 ? Color.yellow : color2;
      GUIUtils.DrawRectWide(axise7, normalized7, right2, (float) size7, _color7);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[4], colorA3, this.hightlightedAxis.z > 0 ? color5 : color4);
      Vector3 axise8 = this.Axises[4];
      Vector3 vector3_13 = Vector3.op_Subtraction(this.Axises[4], this.AxisOrigin);
      Vector3 normalized8 = ((Vector3) ref vector3_13).normalized;
      Vector3 up5 = Vector3.up;
      double size8 = (double) num * 0.125;
      Color _color8 = this.hightlightedAxis.z > 0 ? Color.yellow : color4;
      GUIUtils.DrawRectWide(axise8, normalized8, up5, (float) size8, _color8);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[5], colorA3, this.hightlightedAxis.z < 0 ? color5 : color4);
      Vector3 axise9 = this.Axises[5];
      Vector3 vector3_14 = Vector3.op_Subtraction(this.Axises[5], this.AxisOrigin);
      Vector3 normalized9 = ((Vector3) ref vector3_14).normalized;
      Vector3 up6 = Vector3.up;
      double size9 = (double) num * 0.125;
      Color _color9 = this.hightlightedAxis.z < 0 ? Color.yellow : color4;
      GUIUtils.DrawRectWide(axise9, normalized9, up6, (float) size9, _color9);
    }
    else
    {
      if (GamePrefs.GetInt(EnumGamePrefs.SelectionOperationMode) != 3 || !this.ownerCategory.callback.OnSelectionBoxIsAvailable(this.ownerCategory.name, EnumSelectionBoxAvailabilities.CanMirror))
        return;
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.right)));
      this.Axises.Add(Vector3.op_Subtraction(vector3_4, Vector3.op_Multiply(num, Vector3.right)));
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.up)));
      this.Axises.Add(Vector3.op_Subtraction(vector3_4, Vector3.op_Multiply(num, Vector3.up)));
      this.Axises.Add(Vector3.op_Addition(vector3_4, Vector3.op_Multiply(num, Vector3.forward)));
      this.Axises.Add(Vector3.op_Subtraction(vector3_4, Vector3.op_Multiply(num, Vector3.forward)));
      this.AxisesI.Add(Vector3i.right);
      this.AxisesI.Add(Vector3i.left);
      this.AxisesI.Add(Vector3i.up);
      this.AxisesI.Add(Vector3i.down);
      this.AxisesI.Add(Vector3i.forward);
      this.AxisesI.Add(Vector3i.back);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[0], colorA1, this.hightlightedAxis.x > 0 ? color1 : Color.red);
      GUIUtils.DrawLineWide(Vector3.op_Subtraction(this.Axises[0], Vector3.op_Multiply(Vector3.up, 0.2f)), Vector3.op_Addition(this.Axises[0], Vector3.op_Multiply(Vector3.up, 0.2f)), this.hightlightedAxis.x > 0 ? Color.yellow : Color.red);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[1], colorA1, this.hightlightedAxis.x < 0 ? color1 : Color.red);
      GUIUtils.DrawLineWide(Vector3.op_Subtraction(this.Axises[1], Vector3.op_Multiply(Vector3.up, 0.2f)), Vector3.op_Addition(this.Axises[1], Vector3.op_Multiply(Vector3.up, 0.2f)), this.hightlightedAxis.x < 0 ? Color.yellow : Color.red);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[2], colorA2, this.hightlightedAxis.y > 0 ? color3 : color2);
      GUIUtils.DrawLineWide(Vector3.op_Subtraction(this.Axises[2], Vector3.op_Multiply(Vector3.right, 0.2f)), Vector3.op_Addition(this.Axises[2], Vector3.op_Multiply(Vector3.right, 0.2f)), this.hightlightedAxis.y > 0 ? Color.yellow : color2);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[3], colorA2, this.hightlightedAxis.y < 0 ? color3 : color2);
      GUIUtils.DrawLineWide(Vector3.op_Subtraction(this.Axises[3], Vector3.op_Multiply(Vector3.right, 0.2f)), Vector3.op_Addition(this.Axises[3], Vector3.op_Multiply(Vector3.right, 0.2f)), this.hightlightedAxis.y < 0 ? Color.yellow : color2);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[4], colorA3, this.hightlightedAxis.z > 0 ? color5 : color4);
      GUIUtils.DrawLineWide(Vector3.op_Subtraction(this.Axises[4], Vector3.op_Multiply(Vector3.right, 0.2f)), Vector3.op_Addition(this.Axises[4], Vector3.op_Multiply(Vector3.right, 0.2f)), this.hightlightedAxis.z > 0 ? Color.yellow : color4);
      GUIUtils.DrawLineWide(this.AxisOrigin, this.Axises[5], colorA3, this.hightlightedAxis.z < 0 ? color5 : color4);
      GUIUtils.DrawLineWide(Vector3.op_Subtraction(this.Axises[5], Vector3.op_Multiply(Vector3.right, 0.2f)), Vector3.op_Addition(this.Axises[5], Vector3.op_Multiply(Vector3.right, 0.2f)), this.hightlightedAxis.z < 0 ? Color.yellow : color4);
    }
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void BuildFrame()
  {
    if (Object.op_Inequality((Object) this.frame, (Object) null))
    {
      Object.Destroy((Object) this.frame);
      this.frame = (GameObject) null;
    }
    this.frame = new GameObject("Frame");
    this.frame.transform.parent = ((Component) this).transform;
    this.frame.transform.localScale = Vector3.one;
    this.frame.transform.localPosition = Vector3.zero;
    float num = 0.02f;
    Bounds bounds = this.bounds;
    ((Bounds) ref bounds).center = new Vector3(0.0f, ((Bounds) ref this.bounds).size.y / 2f, 0.0f);
    Mesh mesh = new Mesh();
    mesh.Clear(false);
    List<Vector3> vector3List = new List<Vector3>();
    List<int> intList = new List<int>();
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).min.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).min.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).min.y + num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z + num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x - num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z - num));
    vector3List.Add(new Vector3(((Bounds) ref bounds).max.x + num, ((Bounds) ref bounds).max.y - num, ((Bounds) ref bounds).max.z - num));
    for (int index = 0; index < 12; ++index)
    {
      intList.Add(index * 8);
      intList.Add(index * 8 + 1);
      intList.Add(index * 8 + 2);
      intList.Add(index * 8 + 2);
      intList.Add(index * 8 + 3);
      intList.Add(index * 8);
      intList.Add(index * 8 + 3);
      intList.Add(index * 8 + 2);
      intList.Add(index * 8 + 7);
      intList.Add(index * 8 + 7);
      intList.Add(index * 8 + 4);
      intList.Add(index * 8 + 3);
      intList.Add(index * 8 + 4);
      intList.Add(index * 8 + 7);
      intList.Add(index * 8 + 6);
      intList.Add(index * 8 + 6);
      intList.Add(index * 8 + 5);
      intList.Add(index * 8 + 4);
      intList.Add(index * 8 + 5);
      intList.Add(index * 8 + 6);
      intList.Add(index * 8 + 1);
      intList.Add(index * 8 + 1);
      intList.Add(index * 8);
      intList.Add(index * 8 + 5);
      intList.Add(index * 8 + 1);
      intList.Add(index * 8 + 6);
      intList.Add(index * 8 + 7);
      intList.Add(index * 8 + 7);
      intList.Add(index * 8 + 2);
      intList.Add(index * 8 + 1);
      intList.Add(index * 8 + 5);
      intList.Add(index * 8);
      intList.Add(index * 8 + 3);
      intList.Add(index * 8 + 3);
      intList.Add(index * 8 + 4);
      intList.Add(index * 8 + 5);
    }
    mesh.SetVertices(vector3List);
    mesh.SetIndices(intList.ToArray(), (MeshTopology) 0, 0);
    this.frame.AddComponent<MeshFilter>().mesh = mesh;
    ((Renderer) this.frame.AddComponent<MeshRenderer>()).material = Object.Instantiate<Material>(Resources.Load<Material>("Materials/SleeperVolumeFrame"));
    this.SetFrameColor(this.curFrameColor);
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void addQuad(
    Vector3 _v1,
    Vector2 _uv1,
    Vector3 _v2,
    Vector2 _uv2,
    Vector3 _v3,
    Vector2 _uv3,
    Vector3 _v4,
    Vector2 _uv4,
    BlockFace _face)
  {
    this.m_Vertices.Add(_v1);
    this.m_Vertices.Add(_v2);
    this.m_Vertices.Add(_v3);
    this.m_Vertices.Add(_v4);
    this.m_Uvs.Add(_uv1);
    this.m_Uvs.Add(_uv2);
    this.m_Uvs.Add(_uv3);
    this.m_Uvs.Add(_uv4);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex + 2);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex + 1);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex + 3);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex + 2);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex);
    this.currentChunkMeshIndex += 4;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public void addTriangle(
    Vector3 _v1,
    Vector2 _uv1,
    Vector3 _v2,
    Vector2 _uv2,
    Vector3 _v3,
    Vector2 _uv3,
    BlockFace _face)
  {
    this.m_Vertices.Add(_v1);
    this.m_Vertices.Add(_v2);
    this.m_Vertices.Add(_v3);
    this.m_Uvs.Add(_uv1);
    this.m_Uvs.Add(_uv2);
    this.m_Uvs.Add(_uv3);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex + 2);
    this.subMeshTriangles[(int) _face].Add(this.currentChunkMeshIndex + 1);
    this.currentChunkMeshIndex += 3;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public bool createMesh()
  {
    float num1 = -0.5f;
    float num2 = 0.0f;
    float num3 = -0.5f;
    Bounds bounds = BoundsUtils.BoundsForMinMax(Vector3.zero, Vector3.one);
    float num4 = num1 + ((Bounds) ref bounds).min.x;
    float num5 = num2 + ((Bounds) ref bounds).min.y;
    float num6 = num3 + ((Bounds) ref bounds).min.z;
    float num7 = ((Bounds) ref bounds).max.x - ((Bounds) ref bounds).min.x;
    float num8 = ((Bounds) ref bounds).max.y - ((Bounds) ref bounds).min.y;
    float num9 = ((Bounds) ref bounds).max.z - ((Bounds) ref bounds).min.z;
    for (int index = 0; index < this.subMeshTriangles.Length; ++index)
      this.subMeshTriangles[index].Clear();
    this.m_Uvs.Clear();
    this.m_Vertices.Clear();
    this.currentChunkMeshIndex = 0;
    if (this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockInnerSides)
    {
      this.addQuad(new Vector3(num4, num5, num6), new Vector2(1f, 0.0f), new Vector3(num4, num5 + num8, num6), new Vector2(1f, 1f), new Vector3(num4 + num7, num5 + num8, num6), new Vector2(0.0f, 1f), new Vector3(num4 + num7, num5, num6), new Vector2(0.0f, 0.0f), BlockFace.South);
      this.addQuad(new Vector3(num4, num5, num6 + num9), new Vector2(0.0f, 0.0f), new Vector3(num4, num5 + num8, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4, num5 + num8, num6), new Vector2(1f, 1f), new Vector3(num4, num5, num6), new Vector2(1f, 0.0f), BlockFace.West);
      this.addQuad(new Vector3(num4 + num7, num5, num6 + num9), new Vector2(0.0f, 0.0f), new Vector3(num4 + num7, num5 + num8, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4, num5 + num8, num6 + num9), new Vector2(1f, 1f), new Vector3(num4, num5, num6 + num9), new Vector2(1f, 0.0f), BlockFace.North);
      this.addQuad(new Vector3(num4 + num7, num5, num6), new Vector2(1f, 0.0f), new Vector3(num4 + num7, num5 + num8, num6), new Vector2(1f, 1f), new Vector3(num4 + num7, num5 + num8, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4 + num7, num5, num6 + num9), new Vector2(0.0f, 0.0f), BlockFace.East);
      this.addQuad(new Vector3(num4, num5 + num8, num6), new Vector2(1f, 0.0f), new Vector3(num4, num5 + num8, num6 + num9), new Vector2(1f, 1f), new Vector3(num4 + num7, num5 + num8, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4 + num7, num5 + num8, num6), new Vector2(0.0f, 0.0f), BlockFace.Top);
      this.addQuad(new Vector3(num4 + num7, num5, num6), new Vector2(0.0f, 0.0f), new Vector3(num4 + num7, num5, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4, num5, num6 + num9), new Vector2(1f, 1f), new Vector3(num4, num5, num6), new Vector2(1f, 0.0f), BlockFace.Bottom);
    }
    if (this.focusType == RenderCubeType.FaceS || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
      this.addQuad(new Vector3(num4 + num7, num5, num6), new Vector2(1f, 0.0f), new Vector3(num4 + num7, num5 + num8, num6), new Vector2(1f, 1f), new Vector3(num4, num5 + num8, num6), new Vector2(0.0f, 1f), new Vector3(num4, num5, num6), new Vector2(0.0f, 0.0f), BlockFace.South);
    if (this.focusType == RenderCubeType.FaceW || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
      this.addQuad(new Vector3(num4, num5, num6), new Vector2(0.0f, 0.0f), new Vector3(num4, num5 + num8, num6), new Vector2(0.0f, 1f), new Vector3(num4, num5 + num8, num6 + num9), new Vector2(1f, 1f), new Vector3(num4, num5, num6 + num9), new Vector2(1f, 0.0f), BlockFace.West);
    if (this.focusType == RenderCubeType.FaceN || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
      this.addQuad(new Vector3(num4, num5, num6 + num9), new Vector2(0.0f, 0.0f), new Vector3(num4, num5 + num8, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4 + num7, num5 + num8, num6 + num9), new Vector2(1f, 1f), new Vector3(num4 + num7, num5, num6 + num9), new Vector2(1f, 0.0f), BlockFace.North);
    if (this.focusType == RenderCubeType.FaceE || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
      this.addQuad(new Vector3(num4 + num7, num5, num6 + num9), new Vector2(1f, 0.0f), new Vector3(num4 + num7, num5 + num8, num6 + num9), new Vector2(1f, 1f), new Vector3(num4 + num7, num5 + num8, num6), new Vector2(0.0f, 1f), new Vector3(num4 + num7, num5, num6), new Vector2(0.0f, 0.0f), BlockFace.East);
    if (this.focusType == RenderCubeType.FaceTop || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
      this.addQuad(new Vector3(num4 + num7, num5 + num8, num6), new Vector2(1f, 0.0f), new Vector3(num4 + num7, num5 + num8, num6 + num9), new Vector2(1f, 1f), new Vector3(num4, num5 + num8, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4, num5 + num8, num6), new Vector2(0.0f, 0.0f), BlockFace.Top);
    if (this.focusType == RenderCubeType.FaceBottom || this.focusType == RenderCubeType.FullBlockBothSides || this.focusType == RenderCubeType.FullBlockOuterSides)
      this.addQuad(new Vector3(num4, num5, num6), new Vector2(0.0f, 0.0f), new Vector3(num4, num5, num6 + num9), new Vector2(0.0f, 1f), new Vector3(num4 + num7, num5, num6 + num9), new Vector2(1f, 1f), new Vector3(num4 + num7, num5, num6), new Vector2(1f, 0.0f), BlockFace.Bottom);
    this.m_MeshFilter.mesh.Clear();
    this.m_MeshFilter.mesh.vertices = this.m_Vertices.ToArray();
    if (this.m_Uvs.Count > 0)
      this.m_MeshFilter.mesh.uv = this.m_Uvs.ToArray();
    this.m_MeshFilter.mesh.subMeshCount = this.subMeshTriangles.Length;
    for (int index = 0; index < this.subMeshTriangles.Length; ++index)
      this.m_MeshFilter.mesh.SetTriangles(this.subMeshTriangles[index].ToArray(), index);
    this.m_MeshFilter.mesh.RecalculateNormals();
    if (this.collTag != null)
    {
      ((Component) this.m_MeshFilter).gameObject.AddComponent<MeshCollider>().sharedMesh = this.copyMeshAndAddBackFaces(this.m_MeshFilter.mesh);
      GameObject gameObject = ((Component) this.m_MeshFilter).gameObject;
      gameObject.tag = this.collTag;
      gameObject.layer = this.collLayer;
    }
    return true;
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public Mesh copyMeshAndAddBackFaces(Mesh _mesh)
  {
    Vector3[] vertices = _mesh.vertices;
    List<int> intList = new List<int>();
    for (int index = 0; index < _mesh.subMeshCount; ++index)
    {
      foreach (int triangle in _mesh.GetTriangles(index))
        intList.Add(triangle);
    }
    int count = intList.Count;
    for (int index = 0; index < count; index += 3)
    {
      intList.Add(intList[index]);
      intList.Add(intList[index + 2]);
      intList.Add(intList[index + 1]);
    }
    return new Mesh()
    {
      vertices = vertices,
      triangles = intList.ToArray()
    };
  }

  [PublicizedFrom(EAccessModifier.Private)]
  static SelectionBox()
  {
  }

  [PublicizedFrom(EAccessModifier.Private)]
  public class SizeTextMeshDefinition
  {
    public readonly string Name;
    public readonly Vector3 Position;
    public readonly Vector3 Rotation;
    public readonly char[] Arrows;

    public SizeTextMeshDefinition(
      string _name,
      Vector3 _position,
      Vector3 _rotation,
      char[] _arrows)
    {
      this.Name = _name;
      this.Position = _position;
      this.Rotation = _rotation;
      this.Arrows = _arrows;
    }
  }
}
