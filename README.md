TurtleMesh
==========

TurtleMesh
Turtle is a open-source library for Rhino and Grasshopper containing a definition of an ngon face-vertex mesh. This is most probably the most basic mesh representation, and it is the same as the one used by Rhino, with the exception that this representation fully supports ngons, or faces with an arbitrary number of sides.

This is distributed for free under the terms of the GNU Lesser Public License (see LICENSE.txt). 
It automatically converts Rhino meshes and closed polygons, and possibilities to Join and Explode meshes are available.

Serialization
-------------
To fully support ngon meshes, serialization is readily available. It loads and saves vertices and faces in the .obj file format (a subset of it). Right now only vertices and faces are stored, but in the future also vertex and face normals will be stored.
TurtleMeshes can be already "internalized" in Gh files, making them simple to use and exchange. The goal of this library is to be as open as possible, and be as interchangeable as possible.

Open for any addition
---------------------
It uses interfaces all the time, so you can write your own implementation and even leverage the custom parameter functionality. It will also be made compatible with the next coming version of Weavebird and I hope Starling, Kangaroo, Karamba and all further add-ons using meshes will follow. When source code will be available, I will make it compatible also with Plankton and any other mesh representation that should be necessary.
Add-ons writers are welcome to join and propose changes. Users can use bugtracker functionality.

Preview
-------
Preview works directly and users can see their ngon mesh as though they were native.

Contact
-------
Email Giulio Piacentino: mail at giuliopiacentino d.t com or any of the authors
