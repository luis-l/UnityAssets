using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Bonsai.Core;
using Bonsai.Standard;

public class TestInit : MonoBehaviour
{
    public BonsaiTreeComponent treeComp;
    private BehaviourTree tree;

    // Use this for initialization
    void Start()
    {
        tree = treeComp.Tree;

        tree.Blackboard.Set("Flag1", false);
        tree.Blackboard.Set("Flag2", false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.A)) {
            bool bToggle = !tree.Blackboard.Get<bool>("Flag1");
            tree.Blackboard.Set("Flag1", bToggle);

            tree.Blackboard.Set("Target", transform);
        }

        else if (Input.GetKeyDown(KeyCode.Space)) {

            bool bToggle = !tree.Blackboard.Get<bool>("Flag2");
            tree.Blackboard.Set("Flag2", bToggle);

            tree.Blackboard.Set<Transform>("Target", null);
        }
    }
}
