# unity-mesh-triangulator
A modified version of [alexmuab's unity-mesh-triangulator](https://github.com/alexmuab/unity-mesh-triangulator).

### Usage
```
//Simplest way: finds attached MeshFilter and MeshRenderer
MeshTriangulator.Triangulate(transform);

//More controlled way
MeshTriangulator.Triangulate(transform,meshFilter,meshRenderer);

//You can also specify the force and lifetime applied to the generated triangles 
MeshTriangulator.Triangulate(transform,100,3)
```

## Todo

* Add function to triangulate based on a Mesh and Materials instead of components
* More options, such as controlling whether the triangles use collision
* Adding a mode that doesn't use unity physics, possibly particles of some sort?
* Possibly dropping this in favor of using a shader instead

