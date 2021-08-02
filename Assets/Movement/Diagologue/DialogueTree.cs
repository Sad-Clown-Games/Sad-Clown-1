using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.Experimental.GraphView;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.UIElements;



public class DialogueTree : EditorWindow
{
    private DialogueTreeView tree;
    private string filename;
    [MenuItem("Graph/Dialogue Tree")]
    public static void OpenTreeWindow()
    {
        var window = GetWindow<DialogueTree>();
        window.titleContent = new GUIContent(text: "Dialogue Graph");
    }

    private void OnEnable()
    {
        ConstructTree();
        GenerateTools();
    }
    private void OnDisable()
    {
        rootVisualElement.Remove(tree);
    }

    private void GenerateTools()
    {
        var toolbar = new Toolbar();

        var fileNameTextField = new TextField(label: "Filme Name:");
        fileNameTextField.SetValueWithoutNotify(filename);
        fileNameTextField.MarkDirtyRepaint();
        fileNameTextField.RegisterValueChangedCallback(evt => filename = evt.newValue);

        toolbar.Add(fileNameTextField);

        toolbar.Add(new Button(() => RequestDataOperation(true)) { text = "Save Data" });
        toolbar.Add(new Button(() => RequestDataOperation(false)) { text = "Load Data" });

        var nodeCreateButton = new Button(clickEvent: () =>{tree.CreateNode("DialogueNode");  });
        
        nodeCreateButton.text = "Create Node";
        toolbar.Add(nodeCreateButton);

        rootVisualElement.Add(toolbar);
    }
    private void RequestDataOperation(bool save)
    {
        if (!string.IsNullOrEmpty(filename))
        {
            var saveUtility = GraphSaveUtility.GetInstance(tree);
            if (save)
                saveUtility.SaveGraph(filename);
            else
                saveUtility.LoadNarrative(filename);
        }
        else
        {
            EditorUtility.DisplayDialog("Invalid File name", "Please Enter a valid filename", "OK");
        }
    }
    private void ConstructTree()
    {
        tree = new DialogueTreeView
        {
            name = "Dialogue Tree"
        };

        tree.StretchToParentSize();
        rootVisualElement.Add(tree);
    }
}
