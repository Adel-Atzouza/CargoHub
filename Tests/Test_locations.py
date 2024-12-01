import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/Locations"  # Pas aan op basis van je API-host en poort
headers = {'APIKEY': "Admin"}


def test_get_all_locations():
    response = requests.get(BASE_URL, headers=headers)
    assert response.status_code == 200
    locations = response.json()
    assert isinstance(locations, list)
    assert len(locations) > 0

def test_get_location_by_id():
    location_id = 1  # Vervang met een geldig locatie-ID
    response = requests.get(f"{BASE_URL}/{location_id}")
    assert response.status_code == 200
    location = response.json()
    assert location["id"] == location_id

def test_get_location_by_invalid_id():
    invalid_id = 9999  # Gebruik een ID dat niet bestaat
    response = requests.get(f"{BASE_URL}/{invalid_id}")
    assert response.status_code == 404

def test_remove_location_invalid_id():
    invalid_id = 9999
    response = requests.delete(f"{BASE_URL}/{invalid_id}")
    assert response.status_code == 404
