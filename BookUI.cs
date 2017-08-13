using UnityEngine;
using System.Collections;

public class BookUI : MonoBehaviour {
	private const int MAX_BOOKS = 8;

	private int tot = 0;
	private Texture[] covers = new Texture[MAX_BOOKS];
	private Texture[] contents = new Texture[MAX_BOOKS];

	private float coverOffsetX = Screen.width - 60f;
	private float coverOffsetY = 10f;
	private float coverSizeX = 35f;
	private float coverSizeY = 50f;
	private float coverSpace = 5f;

	private float contentOffsetRightX = Screen.width - 70f;
	private float contentOffsetY = 10f;

	private float zoomPixel = 3f;

	private bool showCursor = false;
	private int selectedBook = -1;

	void Start () {
	
	}

	void Update () {
		showCursor = GetComponent<PlayerController> ().showCursor;
		if (showCursor == false)
			selectedBook = -1;
		coverOffsetX = Screen.width - 60f;
		contentOffsetRightX = Screen.width - 70f;
	}

	void OnGUI() {
		GUI.color = GetMainColor ();

		bool removed = false;
		for (int i = 0; i < tot; i++) {
			Rect rect = new Rect(coverOffsetX, coverOffsetY + i * (coverSizeY + coverSpace), coverSizeX, coverSizeY);
			if (showCursor && MouseHover(rect)) {
				if (Input.GetMouseButton(0)) {
					rect = Zoom(rect, -zoomPixel);
					selectedBook = i;
				} else {
					rect = Zoom(rect, zoomPixel);
				}
			}
			GUI.DrawTexture(rect, covers[i]);
		}

		if (selectedBook != -1) {
			Rect rect = new Rect(contentOffsetRightX - contents[selectedBook].width, contentOffsetY, contents[selectedBook].width, contents[selectedBook].height);
			GUI.DrawTexture(rect, contents[selectedBook]);
			if ((Input.GetMouseButtonDown(0) && !MouseHover(rect)) || Input.GetMouseButtonDown(1))
				selectedBook = -1;
		}
	}

	public void GetABook(Texture cover, Texture content) {
		if (tot == MAX_BOOKS)
			RemoveBook(0);
		covers [tot] = cover;
		contents [tot] = content;
		tot++;
	}

	void RemoveBook(int id) {
		for (int i = id; i + 1 < tot; i++) {
			covers[i] = covers[i + 1];
			contents[i] = contents[i + 1];
		}
		tot--;
	}

	Color GetMainColor() {
		if (showCursor)
			return Color.white;
		return Color.white * 0.5f;
	}

	bool MouseHover(Rect rect) {
		Vector2 mousePosition = Input.mousePosition;
		mousePosition = new Vector2 (mousePosition.x, Screen.height - mousePosition.y);
		if (rect.Contains(mousePosition))
			return true;
		return false;
	}

	Rect Zoom(Rect rect, float pixel) {
		return new Rect(rect.x - pixel, rect.y - pixel, rect.width + 2 * pixel, rect.height + 2 * pixel);
	}
}
