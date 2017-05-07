/**
 * Copyright © 2017 Vitor Rozsa, vitor.rozsa@hotmail.com
 * 
 *	This file is part of Shortest Path.
 *
 *	Shortest Path is free software: you can redistribute it and/or modify
 *	it under the terms of the GNU General Public License as published by
 *	the Free Software Foundation, either version 3 of the License, or
 *	(at your option) any later version.
 *
 *	Shortest Path is distributed in the hope that it will be useful,
 *	but WITHOUT ANY WARRANTY; without even the implied warranty of
 *	MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
 *	GNU General Public License for more details.
 *
 *	You should have received a copy of the GNU General Public License
 *	along with Shortest Path. If not, see<http://www.gnu.org/licenses/>.
 */
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;

namespace CSGameUtils
{
	/// <summary>
	/// Calculate the shortest path between two nodes.
	/// </summary>
	public static class ShortestPath
	{
		static List<SPNode> CopyListItems(List<SPNode> nodesList)
		{
			List<SPNode> newNodesList = new List<SPNode>();

        
			// Create nodes.
			for (int i = 0; i < nodesList.Count; i++) {
				SPNode currNode = nodesList[i];
				SPNode newNode = new SPNode(currNode.ID, currNode.Weight);
				newNodesList.Add(newNode);
			}
        
			// Set neighbors.
			for (int i = 0; i < nodesList.Count; i++) {

				SPNode currNode = nodesList[i];
				SPNode newNode = newNodesList[i];

				for (int j = 0; j < currNode.Neighbors.Count; j++) {
					int newNeighborID = currNode.Neighbors[j].ID;
					newNode.AddNeighbor(newNodesList[newNeighborID]);
				}
			}

			return newNodesList;
		}

		/// <summary>
		/// Find the shortest path between originNodeID and targetNodeID in nodes Graph.
		/// </summary>
		/// <param name="originNodeID">Origin node.</param>
		/// <param name="targetNodeID">Target node.</param>
		/// <param name="nodes">Graph.</param>
		/// <returns></returns>
		public static SPNode[] FindShortestPath(int originNodeID, int targetNodeID, List<SPNode> nodes)
		{
			List<SPNode> nodesToSearch = CopyListItems(nodes);

			List <SPNode> openList = new List<SPNode>();
			List<SPNode> closedList = new List<SPNode>();

			// Origin and target node must be in the graph.
			SPNode originNode = nodesToSearch.Find(x => x.ID == originNodeID);
			Assert.IsNotNull<SPNode>(originNode);
			SPNode targetNode = nodesToSearch.Find(x => x.ID == targetNodeID);
			Assert.IsNotNull<SPNode>(targetNode);

			// Check if target node is conected with any other node.
			if (targetNode.Neighbors.Count == 0) return null;

			// Initialize the open list.
			GetNeighbors(originNode, openList, closedList);
        
			closedList.Add(originNode);
			SPNode currNode = originNode;

			// Do while the target node is found in the closed list.
			while ((CheckNodeInList(targetNode, closedList) != true) && (openList.Count > 0)) {

				SPNode nextNode = GetLeastCostNode(openList);

				currNode = nextNode;
				closedList.Add(currNode);

				GetNeighbors(currNode, openList, closedList);
			}
			//DumpPath(closedList);
			return BuildShortestPathList(targetNode, closedList);
		}

		/// <summary>
		/// Print the shortest path found. Debug method.
		/// </summary>
		public static void DumpPath(List<SPNode> list)
		{
			Debug.Log("Path size: " + list.Count);

			// Forward.
			for (int i = 0; i < list.Count; i++) {
				SPNode curr = list[i];
				Debug.Log("ID: " + curr.ID + " - prev node: " + ((curr.PreviousNode != null)? curr.PreviousNode.ID.ToString() : "none"));
			}
		}

		/// <summary>
		/// Build a list from the shortest path found.
		/// </summary>
		static SPNode[] BuildShortestPathList(SPNode targetNode, List<SPNode> list)
		{
			SPNode prevNode = targetNode.PreviousNode;
			List<SPNode> spList = new List<SPNode>();

			// If the target node wast found, return null.
			if (list.Find(x => x.ID == targetNode.ID) == null) return null;

			spList.Add(targetNode);

			while ((prevNode != null) && (prevNode != prevNode.PreviousNode)) {
				spList.Add(prevNode);
				prevNode = prevNode.PreviousNode;
			}

			spList.Reverse();

			return spList.ToArray();
		}

		/// <summary>
		/// Find the neighbors from a node.
		/// </summary>
		/// <param name="node">The node to be processed.</param>
		/// <param name="openList">The list to receive the neighbors.</param>
		static void GetNeighbors(SPNode node, List<SPNode> openList, List<SPNode> closedList)
		{
			for (int i = 0; i < node.Neighbors.Count; i++) {
				SPNode neighbor = node.Neighbors[i];

				if (closedList.Contains(neighbor)) continue;
				if (openList.Contains(neighbor)) continue;

				neighbor.PreviousNode = node;
				neighbor.WeightToReachFromPreviousNode = node.WeightToReachFromPreviousNode + neighbor.Weight;
                        
				openList.Add(neighbor);
			}
		}

		/// <summary>
		/// Checks if the node is present in the list.
		/// </summary>
		/// <param name="node">The node to look for.</param>
		/// <param name="closedList">The list to be searched.</param>
		/// <returns>true if the node is in the list; false otherwise</returns>
		static bool CheckNodeInList(SPNode node, List<SPNode> closedList)
		{
			SPNode targetNode = closedList.Find(x => x.ID == node.ID);

			return (targetNode != null);
		}

		/// <summary>
		/// Finds the least cost node (remove it from list before returning).
		/// </summary>
		/// <returns>The least cost node in the list.</returns>
		static SPNode GetLeastCostNode(List<SPNode> openList)
		{
			SPNode leastCostNode = openList[0];
			int leastCostNodeWeight = leastCostNode.WeightToReachFromPreviousNode;

			for (int i = 1; i < openList.Count; i++) {
				SPNode nextNode = openList[i];

				int nextNodeWeight = nextNode.WeightToReachFromPreviousNode;
				if (nextNodeWeight < leastCostNodeWeight) {
					leastCostNode = nextNode;
					leastCostNodeWeight = leastCostNode.WeightToReachFromPreviousNode;
				}
			}

			openList.Remove(leastCostNode);

			return leastCostNode;
		}
	}
} // namespace CSGameUtils