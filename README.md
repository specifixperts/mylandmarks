
<h1>Welcome</h1>

My Landmarks retrieves landmark images from Fickr and FourSquare based on a specified location
API Key
To be able to access the api you require key, which you can get from here. Provide the api key in the header for authentication.

    type: apiKey
    name: ApiKey
    in: header


<h1>End Points</h1>
<h2>Locations</h2>

<b>Description:</b>
Returns a list of countries and cities with latitude and longitude values.

Cities are retrieved from GeoDB Cities API
Countries are retrieved from Rest Countries
<b>List</b>
https://mylapi.swe-nam.com/Locations
Retrieve all the saved locations countries and cities.
<b>Response Fields:</b>

    name: string | name of the location.
    latitude: string | latitude.
    longitude: string | longitude
    type: guid | type loaction, {country, city, coord}
    id: guid | photo unique identifier.
    dateCreated: datetime | date and time the location was saved in the database
    dateLastEdited: datetime | date and time when the location record was last edited.
    deleted: bool | true if the location is to be marked as deleted.

Cities

https://mylapi.swe-nam.com/Locations/Cities/{name}
Search for a city using a name or part of the name.
Parameters:

    name: string

Response Fields:

    name: string | name of the location.
    latitude: string | latitude.
    longitude: string | longitude
    type: guid | type loaction, {country, city, coord}
    id: guid | photo unique identifier.
    dateCreated: datetime | date and time the location was saved in the database
    dateLastEdited: datetime | date and time when the location record was last edited.
    deleted: bool | true if the location is to be marked as deleted.

Countries

https://mylapi.swe-nam.com/Locations/Countries/{name}
Search for a country using a name or part of the name.
Parameters:

    name: string

Response Fields:

    name: string | name of the location.
    latitude: string | latitude.
    longitude: string | longitude
    type: guid | type loaction, {country, city, coord}
    id: guid | photo unique identifier.
    dateCreated: datetime | date and time the location was saved in the database
    dateLastEdited: datetime | date and time when the location record was last edited.
    deleted: bool | true if the location is to be marked as deleted.

All

https://mylapi.swe-nam.com/Locations/All/{name}
Search both cities and countries using a name or part of the name.
Parameters:

    name: string

Response Fields:

    name: string | name of the location.
    latitude: string | latitude.
    longitude: string | longitude
    type: guid | type loaction, {country, city, coord}
    id: guid | photo unique identifier.
    dateCreated: datetime | date and time the location was saved in the database
    dateLastEdited: datetime | date and time when the location record was last edited.
    deleted: bool | true if the location is to be marked as deleted.

Photos

Description:
Returns a list of list of landmark images from either Flickr or FourSquare.

FourSquare
FourSquare organises its images based on venues registered in their database. Each photo comes in reference to a venue. For this exercise I used a free registration which limits the images to only one per venue and to get landmark images I had to search for landmark venues, which are not a lot. So there are not a lot of FourSquare results.

Flickr
Flickr is a bit more straight forward when is comes to searching for images, there is quite and array of options to filter by. For the purposes of this exercise the best way to get landmarks that i found was to used tags. Flickr would give a lot more results from almost anywhere in the world.
Get

https://mylapi.swe-nam.com/Photos/{location}
Retrieve saved photos from the given location. You can search using a partial name.
Parameters:

    location: string

Response Fields:

    locationid: guid | unique identifier for the photo's location in the database.
    source: string | name of the service the photo was retrieved from.
    url: string | url path for the photo
    photoId: guid | photo unique identifier.
    locationName: string | name of the location

Search

https://mylapi.swe-nam.com/Photos/Search?locationid={locationid}
This endpoint returns photos searched using a locationid of type GUID. Search for a location and copy the id and paste is in the url.
Parameters:

    locationid: guid

Response Fields:

    locationid: guid | unique identifier for the photo's location in the database.
    source: string | name of the service the photo was retrieved from.
    url: string | url path for the photo
    photoId: guid | photo unique identifier.
    locationName: string | name of the location

Details

https://mylapi.swe-nam.com/Photos/Photo/{id}
This endpoint returns the details of a photo by passing in the id
Parameters:

    id: guid

Response Fields:

    dateTaken: datetime | date and time the photo was taken
    width: int | photo width in pixels
    height: int | photo height in pixels
    sourceId: string | unique identifier for the photo in source database.
    source: string | name of the service the photo was retrieved from.
    url: string | url path for the photo
    id: guid | photo unique identifier.
    dateCreated: datetime | date and time the photo was saved in the database
    dateLastEdited: datetime | date and time when the photo record was last edited.
    deleted: bool | true if the photo is to be marked as deleted.
