using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node : IComparable<Node>
{
    private int row, col, f, g, h, nodeType;
    private Node parent;

    /// <summary>
    /// Constructs a Node with the specified row, column, and type.
    /// </summary>
    /// <param name="r">The row position of the node.</param>
    /// <param name="c">The column position of the node.</param>
    /// <param name="t">The type of the node (0 for traversable, 1 for non-traversable).</param>
    /// row = z col = x
    public Node(int r, int c, int t)
    {
        this.row = r;
        this.col = c;
        this.nodeType = t;
        this.parent = null;
    }

    /// <summary>
    /// Compares this node with another node based on the 'f' value.
    /// Used for ordering nodes in priority queues in the A* algorithm.
    /// </summary>
    /// <param name="other">The other node to compare against.</param>
    /// <returns>
    /// A negative integer, zero, or a positive integer as this node's 
    /// 'f' value is less than, equal to, or greater than the specified node's 'f' value.
    /// </returns>
    public int CompareTo(Node other)
    {
        return this.f.CompareTo(other.f);
    }

    /// <summary>
    /// Sets the 'f' value, which is the sum of 'g' and 'h' values.
    /// </summary>
    public void SetF()
    {
        this.f = this.g + this.h;
    }

    /// <summary>
    /// Sets the 'g' value, which represents the cost from the start node to this node.
    /// </summary>
    public void SetG(int value)
    {
        this.g = value;
    }

    /// <summary>
    /// Sets the 'h' value, which is the estimated cost from this node to the goal.
    /// </summary>
    public void SetH(int value)
    {
        this.h = value;
    }

    /// <summary>
    /// Sets the type of the node.
    /// </summary>
    /// <param name="t">The type to set (0 for traversable, 1 for non-traversable).</param>
    public void SetNodeType(int t)
    {
        this.nodeType = t;
    }

    /// <summary>
    /// Sets the parent node of this node.
    /// </summary>
    /// <param name="n">The parent node.</param>
    public void SetParent(Node n)
    {
        this.parent = n;
    }

    public int GetF() => f;

    public int GetG() => g;

    public int GetH() => h;

    public int GetNodeType() => nodeType;

    public Node GetParent() => parent;

    public int GetRow() => row;

    public int GetCol() => col;

    /// <summary>
    /// Checks if this node is equal to another object.
    /// Nodes are considered equal if they have the same row and column.
    /// </summary>
    /// <param name="obj">The object to compare with.</param>
    /// <returns>True if the nodes have the same row and column, false otherwise.</returns>
    public override bool Equals(object obj)
    {
        if (this == obj)
        {
            return true;
        }
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }
        Node n = (Node)obj;
        return row == n.GetRow() && col == n.GetCol();
    }

    /// <summary>
    /// Returns a string representation of this node, showing its row and column position.
    /// </summary>
    /// <returns>A string representation of the node.</returns>
    public override string ToString()
    {
        return $"Node: [{row},{col}]";
    }

    /// <summary>
    /// Returns the hash code for this node based on its row and column.
    /// </summary>
    /// <returns>The hash code for the node.</returns>
    public override int GetHashCode()
    {
        return HashCode.Combine(row, col);
    }
}



/*
public class Node : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
*/
