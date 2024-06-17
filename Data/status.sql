DROP TABLE IF EXISTS "status";
CREATE TABLE "status" (
    "id" SERIAL8 PRIMARY KEY,
    "posted_by_user_id" INT8 NOT NULL REFERENCES users(id) ON DELETE CASCADE,
    "media_url" VARCHAR(255) NOT NULL,
    "description" VARCHAR(255) NOT NULL,
    "created_at" TIMESTAMPTZ DEFAULT NOW()
);

DROP TABLE IF EXISTS "status_target_users";
CREATE TABLE "status_target_users" (
    "status_id" INT8 REFERENCES status(id) ON DELETE CASCADE,
    "user_id" INT8 REFERENCES users(id),
    "is_seen" BOOL DEFAULT FALSE
);