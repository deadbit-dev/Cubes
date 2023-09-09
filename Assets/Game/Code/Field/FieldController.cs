using System.Collections;
using UnityEngine;
using DG.Tweening;
using JoyTeam.Core;

namespace JoyTeam.Game
{
    public class FieldController : Singleton<FieldController>
    {   
        public Field Field;

        public override void Init()
        {
            Field.transform.localScale = new Vector3(Config.Level.Width * Field.XStep, 1, Config.Level.Height * Field.ZStep);
            
            Field.Grid = new GridService<Unit?>(Config.Level.Width, Config.Level.Height);
            
            for (var row = 0; row < Config.Level.Height; row++)
            {
                for (var column = 0; column < Config.Level.Width; column++)
                {
                    var unitData = Config.Level.Data[row * Config.Level.Width + column];

                    if(unitData == 0) continue;
                    
                    var view = Instantiate(Resources.Load<UnitView>(Config.ViewResourceByData[unitData]));
                    view.transform.parent = Field.transform;
                    view.transform.position = new Vector3(column * Field.XStep, Field.transform.position.y, row * Field.ZStep);

                    Field.Grid.AddValue(
                        new Vector2Int { x = column, y = row },
                        new Unit {data = unitData, view = view }
                    );
                }
            }
        }

        public IEnumerator MoveUnitsRight()
        {
            GameController.SetInput(false);
            var hasChanges = true;

            while(hasChanges) 
            {
                hasChanges = false;

                for (int y = 0; y < Field.Grid.Height; y++)
                {
                    for (int x = Field.Grid.Width - 1; x > 0; x--)
                    {
                        var currentCoord = new Vector2Int(x, y);
                        var neighbourCoord = currentCoord + Vector2Int.left;

                        hasChanges = TryMoveOrMergeUnits(
                           currentCoord,
                           neighbourCoord 
                        ) ? true : hasChanges;
                    }  
                }

                yield return new WaitForSeconds(Config.MoveAndMergeDuration);
            }

            GameController.SetInput(true);
        }

        public IEnumerator MoveUnitsLeft()
        {
            GameController.SetInput(false);
            var hasChanges = true;
            
            while(hasChanges) 
            {
                hasChanges = false;
                
                for (int y = 0; y < Field.Grid.Height; y++)
                {
                    for (int x = 0; x < Field.Grid.Width - 1; x++)
                    {
                        var currentCoord = new Vector2Int(x, y);
                        var neighbourCoord = currentCoord + Vector2Int.right;

                        hasChanges = TryMoveOrMergeUnits(
                            currentCoord,
                            neighbourCoord
                        ) ? true : hasChanges;
                    }  
                }

                yield return new WaitForSeconds(Config.MoveAndMergeDuration);
            }

            GameController.SetInput(true);
        }

        public IEnumerator MoveUnitsDown()
        {
            GameController.SetInput(false);
            var hasChanges = true;

            while(hasChanges) 
            {
                hasChanges = false;
                
                for (int x = 0; x < Field.Grid.Width; x++)
                {
                    for (int y = Field.Grid.Height - 1; y > 0; y--)
                    {
                        var currentCoord = new Vector2Int(x, y);
                        var neighbourCoord = currentCoord + Vector2Int.down;

                        hasChanges = TryMoveOrMergeUnits(
                            currentCoord,
                            neighbourCoord
                        ) ? true : hasChanges;
                    }
                }

                yield return new WaitForSeconds(Config.MoveAndMergeDuration);
            }

            GameController.SetInput(true);
        }

        public IEnumerator MoveUnitsUp()
        {
            GameController.SetInput(false);
            var hasChanges = true;

            while(hasChanges) 
            {
                hasChanges = false;

                for (int x = 0; x < Field.Grid.Width; x++)
                {
                    for (int y = 0; y < Field.Grid.Height - 1; y++)
                    {
                        var currentCoord = new Vector2Int(x, y);
                        var neighbourCoord = currentCoord + Vector2Int.up;

                        hasChanges = TryMoveOrMergeUnits(
                           currentCoord,
                           neighbourCoord 
                        ) ? true : hasChanges;
                    }
                }

                yield return new WaitForSeconds(Config.MoveAndMergeDuration);
            }

            GameController.SetInput(true);
        }

        private bool TryMoveOrMergeUnits(Vector2Int currentCoord, Vector2Int neighbourCoord)
        {
            var currentUnit = Field.Grid.GetValue(currentCoord);
            var neighbourUnit = Field.Grid.GetValue(neighbourCoord);

            var currentIsFree = (currentUnit is null);
            var neighbourIsFree = (neighbourUnit is null);

            var currentData = currentUnit?.data;
            var neighbourData = neighbourUnit?.data;

            var currentIsWall = (currentData == -1);
            var neighbourIsWall = (neighbourData == -1);

            var neighbourIsNotEqualsCurrent = !currentIsFree && (neighbourData != currentData);

            if (currentIsWall || neighbourIsWall || neighbourIsFree || neighbourIsNotEqualsCurrent) return false;

            var targetPos = Field.Grid.GridToWorld(currentCoord.x, currentCoord.y, Field.transform, Field.XStep, Field.ZStep);

            if (currentIsFree)
            {
                Field.Grid.AddValue(currentCoord, neighbourUnit);
                Field.Grid.AddValue(neighbourCoord, null);

                // TODO: move to view
                var direction = (Vector3) (targetPos - neighbourUnit?.view.transform.position)?.normalized;
                var angle = Vector3.Cross(direction, Vector3.up).normalized * 5.0f;

                neighbourUnit?.view.body.DORotate(angle, 0.5f);
                neighbourUnit?.view.transform.DOMove(targetPos, Config.MoveAndMergeDuration).OnComplete(() => {
                    neighbourUnit?.view.body.DORotate(-angle * 2, 0.3f).OnComplete(() => {
                        neighbourUnit?.view.body.DORotate(Vector3.zero, 0.3f);
                    });
                });
            }
            else
            {
                var data = (int) currentUnit?.data + 1;
                
                Field.Grid.AddValue(currentCoord, new Unit {data = data, view = null });
                Field.Grid.AddValue(neighbourCoord, null);

                // TODO: move to view
                var direction = (Vector3) (targetPos - neighbourUnit?.view.transform.position)?.normalized;
                var angle = Vector3.Cross(direction, Vector3.up).normalized * 5.0f;

                neighbourUnit?.view.body.DORotate(angle, 0.5f);
                neighbourUnit?.view.transform.DOMove(targetPos, Config.MoveAndMergeDuration).OnComplete(() => {
                    var view = Instantiate(Resources.Load<UnitView>(Config.ViewResourceByData[data]));
                    var mergeEffect = Instantiate(Resources.Load<GameObject>("Units/MergeEffect"));

                    view.transform.parent = Field.transform;
                    view.transform.position = targetPos;
                    mergeEffect.transform.parent = Field.transform;
                    mergeEffect.transform.position = targetPos;

                    Destroy(currentUnit?.view.gameObject);
                    Destroy(neighbourUnit?.view.gameObject);

                    Field.Grid.AddValue(currentCoord, new Unit {data = data, view = view});
                    
                    view.body.DORotate(-angle, 0.15f).OnComplete(() => {
                        view.body.DORotate(Vector3.zero, 0.15f);
                    });
                });
            }

            return true;
        }
    }   
}