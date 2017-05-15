using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class maze : MonoBehaviour {

	[System.Serializable]
	public class Pass{
		public bool visitado;
		public GameObject norte;
		public GameObject sur;
		public GameObject este;
		public GameObject oeste;
	}

	public GameObject pared;
	public float tamanoPared = 1.0f;
	public int xSize = 5;
	public int ySize = 5;
	private Vector3 posInicial;
	private GameObject wallHolder;
	public Pass[] cells;
	public int currentCell = 0;
	private int totalCells;
	private int visitedCells= 0;
	private bool startedBuilding = false;
	private int currentNeighbour = 0;
	private List<int> lastCells;
	private int backingUp = 0;
	private int wallToBreak = 0;



	// Use this for initialization
	void Start () {
		Laberinto ();
	}

	void Laberinto(){
		wallHolder = new GameObject ();
		wallHolder.name = "maze";
		posInicial = new Vector3 ((-xSize / 2) + tamanoPared / 2, 0.0f, (-ySize / 2) + tamanoPared / 2);
		Vector3 myPos = posInicial;
		GameObject paredTemporal;

		// en x
		for (int i = 0; i < ySize; i++) {
			for (int j = 0; j <= xSize; j++) {
				myPos = new Vector3 (posInicial.x + (j * tamanoPared) - tamanoPared / 2, 0.0f, posInicial.z + (i * tamanoPared) - tamanoPared / 2);
				paredTemporal = Instantiate (pared, myPos, Quaternion.identity) as GameObject;
				paredTemporal.transform.parent = wallHolder.transform;
			}
		}

		// en y
		for (int i = 0; i <= ySize; i++) {
			for (int j = 0; j < xSize; j++) {
				myPos = new Vector3 (posInicial.x + (j * tamanoPared), 0.0f, posInicial.z + (i * tamanoPared) - tamanoPared);
				paredTemporal = Instantiate (pared, myPos, Quaternion.Euler(0.0f, 90.0f, 0.0f)) as GameObject;
				paredTemporal.transform.parent = wallHolder.transform;
			}
		}
		Pasos ();
	}

	void Pasos(){
		lastCells = new List<int>();
		lastCells.Clear ();
		totalCells = xSize * ySize;
		GameObject[] AllParedes;
		int children = wallHolder.transform.childCount;
		AllParedes = new GameObject[children];
		cells = new Pass[xSize * ySize];
		int eastWestProcess = 0;
		int childProcess = 0;
		int termcount = 0;

		for (int i = 0; i < children; i++) {
			AllParedes [i] = wallHolder.transform.GetChild (i).gameObject;
		}

		for (int cellProcess = 0; cellProcess < cells.Length; cellProcess++) {
			cells[cellProcess] = new Pass();
			cells[cellProcess].este = AllParedes [eastWestProcess];
			cells [cellProcess].sur = AllParedes [childProcess + (xSize + 1) * ySize];
			if (termcount == xSize) {
				eastWestProcess += 2;
				termcount = 0;
			} else {
				eastWestProcess++;
			}
			termcount++;
			childProcess++;
			cells [cellProcess].oeste = AllParedes [eastWestProcess];
			cells [cellProcess].norte = AllParedes [(childProcess + (xSize + 1) * ySize) + xSize - 1];
		}
		createLaberinto ();
	}

	void createLaberinto(){
		if (visitedCells < totalCells) {
			if (startedBuilding) {
				vecino ();
				if (cells [currentNeighbour].visitado == false && cells [currentCell].visitado == true) {
					BreakWall ();
					cells[currentNeighbour].visitado = true;
					visitedCells++;
					lastCells.Add (currentCell);
					currentCell = currentNeighbour;
					if (lastCells.Count > 0) {
						backingUp = lastCells.Count - 1;
					}


				}
					
			} else {
				currentCell = Random.Range (0, totalCells);
				cells [currentCell].visitado = true;
				visitedCells++;
				startedBuilding = true;
			}
			Invoke ("createLaberinto", 0.0f);
		}
		
	}

	void BreakWall(){
		switch (wallToBreak) {
		case 1: Destroy(cells[currentCell].norte);
			break;
		case 2: Destroy(cells[currentCell].este);
			break;
		case 3: Destroy(cells[currentCell].oeste);
			break;
		case 4: Destroy(cells[currentCell].sur);
			break;
		}
		
	}

	void vecino(){
		int lenght = 0;
		int[] vecinos = new int[4];
		int[] conectingWalls = new int[4];
		int check = 0;
		check = ((currentCell + 1) / xSize);
		check -= 1;
		check *= xSize;
		check += xSize;

		//oeste
		if (currentCell + 1 < totalCells && (currentCell + 1) != check) {
			if (cells [currentCell + 1].visitado == false) {
				vecinos [lenght] = currentCell + 1;
				conectingWalls [lenght] = 3;
				lenght++;
			}
		}

		//este
		if (currentCell + 1 >= 0 && currentCell != check) {
			if (cells [currentCell - 1].visitado == false) {
				vecinos [lenght] = currentCell - 1;
				conectingWalls [lenght] = 2;
				lenght++;
			}
		}

		//norte
		if (currentCell + xSize < totalCells) {
			if (cells [currentCell + xSize].visitado == false) {
				vecinos [lenght] = currentCell + xSize;
				conectingWalls [lenght] = 1;
				lenght++;
			}
		}

		//sur
		if (currentCell - xSize >=0) {
			if (cells [currentCell - xSize].visitado == false) {
				vecinos [lenght] = currentCell - xSize;
				conectingWalls [lenght] = 4;
				lenght++;
			}
		}

		if (lenght != 0) {
			int theChooseOne = Random.Range (0, lenght);
			currentNeighbour = vecinos [theChooseOne];
			wallToBreak = conectingWalls [theChooseOne];
		} else {
			if (backingUp > 0) {
				currentCell = lastCells [backingUp];
				backingUp--;
			}
		}
	}

}
