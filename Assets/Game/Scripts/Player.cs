using UnityEngine;

namespace Assets.Game.Scripts
{
    public class Player : MonoBehaviour
    {
        [Header("World References")]
        public int X;
        public int Z;
        public World World;

        [Header("Player Values")]
        [SerializeField] float stepSpeed = 1f;
        [SerializeField] bool hasControl = true;

        private Vector3 targetPosition;

        private void Start()
        {
            var tile = World.GetTileAt(X, Z);
            this.transform.position = tile.transform.position + new Vector3(0, 0.55f);
            targetPosition = transform.position;
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space)) { hasControl = !hasControl; }

            if (transform.position != targetPosition)
            {
                float step = stepSpeed * Time.deltaTime;
                transform.position = Vector3.MoveTowards(transform.position, targetPosition, step);
                return;
            }

            if (!hasControl) { return; }
            Movement();
        }

        private void Movement()
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                Move(0, 1);
            }
            else if (Input.GetKeyDown(KeyCode.A))
            {
                Move(-1, 0);
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                Move(0, -1);
            }
            else if (Input.GetKeyDown(KeyCode.D))
            {
                Move(1, 0);
            }
        }

        public void Move(int horizontal, int vertical)
        {
            var tileObject = World.GetTileAt(X + horizontal, Z + vertical);
            if (tileObject is null)
            {
                return;
            }

            targetPosition = tileObject.transform.position + new Vector3(0, 0.55f);

            var tile = tileObject.GetComponent<WorldTile>();
            this.X = tile.X;
            this.Z = tile.Z;
        }
    }
}
