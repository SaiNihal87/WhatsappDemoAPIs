DROP TABLE IF EXISTS groups;

CREATE TABLE "groups" (
    "id" SERIAL8 PRIMARY KEY,
    "name" VARCHAR(20) NOT NULL,
    "description" VARCHAR(20) NOT NULL,
    "created_by_user_id" INT8 NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    "is_public" BOOL DEFAULT FALSE,
    "created_at" TIMESTAMPTZ DEFAULT NOW(),
    "updated_at" TIMESTAMPTZ
);

DROP TABLE IF EXISTS group_members;

CREATE TABLE "group_members" (
    "group_id" INT8 REFERENCES groups(id) ON DELETE CASCADE,
    "user_id" INT8 REFERENCES users(id),
    "is_admin" BOOL
);

