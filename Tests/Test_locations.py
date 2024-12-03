import requests
import pytest

BASE_URL = "http://localhost:3000/api/v1/Locations"
headers = {'APIKEY': "Admin"}


def cleanup_test_data(test_name):
    response = requests.get(BASE_URL, headers=headers)
    if response.status_code == 200:
        locations = response.json()
        for location in locations:
            # Controleer of het een testdata-item is (bijvoorbeeld gebaseerd op een naam of code)
            if "Test" in location["name"] or location["code"] == "TST":
                delete_response = requests.delete(f"{BASE_URL}/{location['id']}", headers=headers)
                assert delete_response.status_code in [200, 204], f"Cleanup failed in {test_name}"


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


def test_add_location():
    new_location = {
        "name": "Test Location",
        "code": "TST",
        "warehouseId": 1,
    }
    response = requests.post(BASE_URL, json=new_location, headers=headers)
    assert response.status_code == 200

    # Controleer of de response JSON is of platte tekst
    if response.headers.get('Content-Type') == 'application/json':
        location = response.json()
        assert location["name"] == new_location["name"]
        assert location["code"] == new_location["code"]
        assert location["warehouseId"] == new_location["warehouseId"]
    else:
        assert response.text == "Locatie succesvol toegevoegd."

def test_update_location():
    response = requests.get(BASE_URL, headers=headers)
    assert response.status_code == 200, f"Failed to fetch locations: {response.text}"

    locations = response.json()
    location_id = None

    # Zoek het testrecord op basis van naam of code
    for location in locations:
        if "Test" in location["name"] or location["code"] == "TST":
            location_id = location["id"]
            break

    assert location_id is not None, "No test location found to update."

    updated_location = {
        "name": "Test Location Updated",
        "code": "TST",
        "warehouseId": 1,
    }
    response = requests.put(f"{BASE_URL}/{location_id}", json=updated_location, headers=headers)
    assert response.status_code == 200

    # Controleer of de response JSON is of platte tekst
    if response.headers.get('Content-Type') == 'application/json':
        location = response.json()
        assert location["name"] == updated_location["name"]
        assert location["code"] == updated_location["code"]
        assert location["warehouseId"] == updated_location["warehouseId"]
    else:
        assert response.text == "Locatie succesvol bijgewerkt."

    # Cleanup na test
    cleanup_test_data("test_update_location")

def test_get_location_by_invalid_id():
    invalid_id = 9999  # Gebruik een ID dat niet bestaat
    response = requests.get(f"{BASE_URL}/{invalid_id}")
    assert response.status_code == 404


def test_remove_location_invalid_id():
    invalid_id = 9999
    response = requests.delete(f"{BASE_URL}/{invalid_id}")
    assert response.status_code == 404
