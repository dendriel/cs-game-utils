Shortest Path

Files:
+ ShortestPath.cs
+ SPNode.cs
+ SPTest.cs

This is an implementation of the shortest path algorithm. It is aimed for "noded maps", that is, maps in which the characters must walk over "nodes" (or tiles in tiled maps).
This means that you don't setup the weight in every connection between nodes. You just set the weight of nodes and its neighbors.

For example, if we have a scenario with grass and swamp. The character may walk over the swamp, but it will prefer to walk over grass. In this case, we just must set a heavier weight for the swamp. Lets say, grass is 1 and swamp is 5. See the grap bellow:

```
G----G----G
#    | \  |
#    |  \ |
S ## G----G
#    #    |
#    #    |
G ## S ###G
```
In the graph above, G is grass and S is swamp. Every connection with a swamp will inherit its weight (5), so these connections are marked as #.

When you go from grass to swamp, the weight will be 5. When you go from swamp to grass, the weight will be 1.

The grass connections are lighter, so the shortest path will be the following (marked with '+'):

```
G++++G----G
#    | +  |
#    |  + |
S ## G----G
#    #    +
#    #    +
G ## S ###G
```

- Check out SPTest.cs for more examples and to find out how to create these graphs. The example above corresponds to TestC() from SPTest.